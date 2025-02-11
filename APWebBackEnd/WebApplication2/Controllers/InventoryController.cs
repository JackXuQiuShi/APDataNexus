
using APWeb.Models;
using APWeb.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;


namespace APWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(IConfiguration configuration, IGeneralService generalService, ApplicationDbContext context) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IGeneralService _generalService = generalService;

        /*private readonly string sqlDataSource = configuration.GetConnectionString("PRIS_ALP_test");*/
        private readonly string sqlDataSource = configuration.GetConnectionString("inventoryCon_current");
        private readonly ApplicationDbContext _context = context;


        [HttpGet("getInventoryByLocation")]
        public async Task<JsonResult> GetInventoryByLocation(string Location)
        {
           /* string query = "SELECT * FROM dbo.Storage " +
                "WHERE LocationOnly = 1 AND Flag = 1 AND Location = @Location " +
                "ORDER BY InspectedDate DESC;";*/

            string query = "SELECT * FROM (" +
                "SELECT *,ROW_NUMBER() OVER (PARTITION BY Product_ID ORDER BY InspectedDate DESC) " +
                "as rn FROM dbo.Storage WHERE LocationOnly = 1 AND Flag = 1 AND Location = @Location ) " +
                "as subquery WHERE rn = 1 ORDER BY InspectedDate DESC;";

            SqlParameter[] parameters = [
                new SqlParameter("@Location", SqlDbType.VarChar) { Value = Location }
            ];
            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpGet("getProductLocation")]
        public async Task<JsonResult> GetProductLocation(string ProductID)
        {
            string query = "SELECT Product_ID, Location, MIN(InspectedDate) AS InspectedDate FROM [Storage] " +
                "WHERE Flag = 1 AND LocationOnly = 1 AND Product_ID = @ProductID " +
                "GROUP BY Product_ID, Location";

            SqlParameter[] parameters = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID }
            ];
            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        /*      [HttpGet("getProductSupplier")]
              public async Task<JsonResult> GetProductSupplier(string ProductID)
              {
                  string query = "SELECT TOP 1 Suppliers.CompanyName, POs.Supplier_ID FROM POs " +
                      "LEFT JOIN Suppliers ON POs.Supplier_ID = Suppliers.Supplier_ID " +
                      "LEFT JOIN PO_Details ON POs.PO_ID = PO_Details.PO_ID " +
                      "WHERE PO_Details.Product_ID = @ProductID " +
                      "ORDER BY POs.PODraftDate DESC;";

                  SqlParameter[] parameters = [
                      new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID }
                  ];

                  DataTable result = await ExecuteQueryAsync(query, parameters);
                  return new JsonResult(result);
              }*/

        //[HttpGet("getNeedReturnListSuppliers")]
        //public async Task<JsonResult> GetNeedReturnListSuppliers()
        //{
        //    string query = "SELECT SupplierID, CompanyName AS SupplierName " +
        //        "FROM dbo.StorageLocation " +
        //        "LEFT JOIN Suppliers ON Suppliers.Supplier_ID = StorageLocation.SupplierID " +
        //        "WHERE Active = 1 AND NeedReturn = 1 " +
        //        "GROUP BY StorageLocation.SupplierID, Suppliers.CompanyName " +
        //        "ORDER BY Suppliers.CompanyName;";

        //    SqlParameter[] parameters = [
        //    ];

        //    DataTable result = await ExecuteQueryAsync(query, parameters);
        //    return new JsonResult(result);
        //}

        [HttpGet("getReturnSuppliers")]
        public async Task<JsonResult> GetReturnSuppliers(int StoreID)
        {
            string query = "SELECT StorageReturn.SupplierID, Suppliers.CompanyName " +
                "FROM dbo.StorageReturn " +
                "LEFT JOIN Suppliers ON Suppliers.Supplier_ID = StorageReturn.SupplierID " +
                "WHERE StoreID = @StoreID AND StatusID = 0 AND ReturnQuantity > 0 " +
                "GROUP BY StorageReturn.SupplierID, Suppliers.CompanyName " +
                "ORDER BY Suppliers.CompanyName;";

            SqlParameter[] parameters = [
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

       /* [HttpGet("getInfo")]
        public async Task<JsonResult> GetInfo(string ProductID)
        {
            string query = "SELECT P.Prod_Name, P.Tax1App, PP.RegPrice, PD.ReceivingDate AS Latest_ReceivingDate, PD.PriceReceived,PD.UnitsPerPackage " +
                "FROM dbo.Products P " +
                "LEFT JOIN dbo.ProductPrice PP ON P.Prod_Num = PP.ProdNum " +
                "LEFT JOIN (SELECT Product_ID, ReceivingDate, PriceReceived, UnitsPerPackage, ROW_NUMBER() OVER (PARTITION BY Product_ID ORDER BY ReceivingDate DESC) AS RowNum FROM dbo.PO_Details) PD ON P.Prod_Num = PD.Product_ID AND PD.RowNum = 1 " +
                "WHERE P.Prod_Num = @ProductID;";


            SqlParameter[] parameters = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }*/

        /*[HttpGet("getInfoFromPOS")]
        public async Task<JsonResult> GetInfoFromPOS(string ProductID)
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today.AddDays(-90);

            string query = "SELECT SUM(Quantity) AS Total_QTY FROM POS_Sales " +
                "WHERE Product_ID = @ProductID AND Date >= @startDate " +
                "GROUP BY Product_ID";

            SqlParameter[] parameters = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
                new SqlParameter("@startDate ", SqlDbType.DateTime) { Value = startDate  }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }*/

        [HttpGet("getInfoFromPOS306090")]
        public async Task<JsonResult> GetInfoFromPOS306090(string ProductID)
        {
            DateTime today = DateTime.Today;
            DateTime startDate30 = today.AddDays(-30);
            DateTime startDate60 = today.AddDays(-60);
            DateTime startDate90 = today.AddDays(-90);

            string query30 = "SELECT SUM(Quantity) AS Total_QTY_30 FROM POS_Sales " +
                "WHERE Product_ID = @ProductID AND Date >= @startDate30 AND Date < @today " +
                "GROUP BY Product_ID";

            string query60 = "SELECT SUM(Quantity) AS Total_QTY_60 FROM POS_Sales " +
                "WHERE Product_ID = @ProductID AND Date >= @startDate60 AND Date < @startDate30 " +
                "GROUP BY Product_ID";

            string query90 = "SELECT SUM(Quantity) AS Total_QTY_90 FROM POS_Sales " +
                "WHERE Product_ID = @ProductID AND Date >= @startDate90 AND Date < @startDate60 " +
                "GROUP BY Product_ID";

            SqlParameter[] parameters30 = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
                new SqlParameter("@startDate30", SqlDbType.DateTime) { Value = startDate30 },
                new SqlParameter("@today", SqlDbType.DateTime) { Value = today }
            ];

            SqlParameter[] parameters60 = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
                new SqlParameter("@startDate60", SqlDbType.DateTime) { Value = startDate60 },
                new SqlParameter("@startDate30", SqlDbType.DateTime) { Value = startDate30 }
            ];

            SqlParameter[] parameters90 = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
                new SqlParameter("@startDate90", SqlDbType.DateTime) { Value = startDate90 },
                new SqlParameter("@startDate60", SqlDbType.DateTime) { Value = startDate60 }
            ];

            DataTable result30 = await ExecuteQueryAsync(query30, parameters30);
            DataTable result60 = await ExecuteQueryAsync(query60, parameters60);
            DataTable result90 = await ExecuteQueryAsync(query90, parameters90);

            // Create a combined result table
            DataTable combinedResult = new DataTable();
            combinedResult.Columns.Add("Total_QTY_30", typeof(int));
            combinedResult.Columns.Add("Total_QTY_60", typeof(int));
            combinedResult.Columns.Add("Total_QTY_90", typeof(int));

            DataRow row = combinedResult.NewRow();
            row["Total_QTY_30"] = result30.Rows.Count > 0 ? result30.Rows[0]["Total_QTY_30"] : 0;
            row["Total_QTY_60"] = result60.Rows.Count > 0 ? result60.Rows[0]["Total_QTY_60"] : 0;
            row["Total_QTY_90"] = result90.Rows.Count > 0 ? result90.Rows[0]["Total_QTY_90"] : 0;

            combinedResult.Rows.Add(row);

            return new JsonResult(combinedResult);
        }

   /*     [HttpGet("getLastScanDate")]
        public async Task<JsonResult> GetLastScanDate(string ProductID, int StoreID)
        {
            string query = "SELECT TOP 1 InspectedDate FROM dbo.Storage " +
                "WHERE Product_ID = @ProductID AND Store_ID = @StoreID AND Flag = 1 AND LocationOnly = 1" +
                "ORDER BY InspectedDate DESC;";

            SqlParameter[] parameters = [
                new SqlParameter("@ProductID", ProductID),
                new SqlParameter("@StoreID", StoreID)
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }*/

        [HttpGet("getLastScanInfo")]
        public async Task<JsonResult> GetLastScanInfo(string ProductID, int StoreID)
        {
            string query = "SELECT TOP 1 LatestDate, Location FROM dbo.StorageLocation " +
                "WHERE ProductID = @ProductID AND StoreID = @StoreID " +
                "ORDER BY LatestDate DESC;";

            SqlParameter[] parameters = [
                new SqlParameter("@ProductID", ProductID),
                new SqlParameter("@StoreID", StoreID)
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        //[HttpGet("getReturnList")]
        //public async Task<JsonResult> GetReturnList(int? SupplierID)
        //{
        //    string query;
        //    SqlParameter[] parameters;

        //    if (SupplierID.HasValue)
        //    {
        //        query = "SELECT ProductID, Location, ProductName, SupplierID, CompanyName AS SupplierName FROM dbo.StorageLocation sl " +
        //            "LEFT JOIN Suppliers s ON s.Supplier_ID = sl.SupplierID " +
        //            "WHERE Active = 1 AND NeedReturn = 1 AND SupplierID = @SupplierID " +
        //            "ORDER BY SupplierID, LatestDate DESC";

        //        parameters = [
        //            new SqlParameter("@SupplierID", SqlDbType.Int) { Value = SupplierID }
        //        ];
        //    }
        //    else {
        //        query = "SELECT ProductID, Location, ProductName, SupplierID, CompanyName AS SupplierName FROM dbo.StorageLocation sl " +
        //            "LEFT JOIN Suppliers s ON s.Supplier_ID = sl.SupplierID " +
        //            "WHERE Active = 1 AND NeedReturn = 1 " +
        //            "ORDER BY SupplierID, LatestDate DESC";

        //        parameters = [];
        //    }

        //    DataTable result = await ExecuteQueryAsync(query, parameters);
        //    return new JsonResult(result);
        //}
    

        [HttpGet("getReturnInfo")]
        public async Task<JsonResult> GetReturnInfo(string ProductID, int StoreID)
        {
            string query = "SELECT TOP 1 ReturnQuantity, StatusID, ReturnID FROM dbo.StorageReturn WHERE ProductID = @ProductID AND StoreID = @StoreID " +
                "ORDER BY Date DESC";

            SqlParameter[] parameters = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpGet("getReturnData")]
        public async Task<JsonResult> GetReturnData(int SupplierID, int StoreID)
        {
            string query = "SELECT a.*, b.CompanyName FROM dbo.StorageReturn a " +
                "LEFT JOIN Suppliers b ON a.SupplierID = b.Supplier_id " +
                "WHERE SupplierID = @SupplierID AND StoreID = @StoreID AND StatusID = 0 AND ReturnQuantity > 0 " +
                "ORDER BY Date DESC";

            SqlParameter[] parameters = [
                new SqlParameter("@SupplierID", SqlDbType.VarChar) { Value = SupplierID },
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpGet("getCreditList")]
        public async Task<JsonResult> GetCreditList(int StoreID)
        {
            DateTime today = DateTime.Today;
            DateTime date = today.AddDays(-60);

            string query = "SELECT DISTINCT CreditNumber, PrintDate, a.SupplierID, b.CompanyName FROM dbo.StorageReturn a " +
                "LEFT JOIN Suppliers b ON a.SupplierID = b.Supplier_id " +
                "WHERE StatusID = 1 AND StoreID = @StoreID AND PrintDate > @Date " +
                "ORDER BY PrintDate DESC; ";

            SqlParameter[] parameters = [
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID },
                new SqlParameter("@Date", SqlDbType.DateTime) { Value = date }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpGet("getCreditByCreditNumber")]
        public async Task<JsonResult> GetCreditByCreditNumber(string CreditNumber)
        {
            string query = "SELECT a.*, b.CompanyName FROM dbo.StorageReturn a " +
                "LEFT JOIN Suppliers b ON a.SupplierID = b.Supplier_id " +
                "WHERE CreditNumber = @CreditNumber  " +
                "ORDER BY Date DESC";

            SqlParameter[] parameters = [
                new SqlParameter("@CreditNumber", SqlDbType.VarChar) { Value = CreditNumber }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpPost("insertInventory")]
        public async Task<IActionResult> InsertInventory(Storage inventory)
        {
            string query = "INSERT INTO dbo.Storage(Store_ID,Product_ID,Location,Units,InspectedBy,InspectedDate,Active,LocationOnly,Flag) " +
                           "VALUES (@Store_ID,@Product_ID,@Location,@Units,@InspectedBy,@InspectedDate,1,1,1)";

            var normalizedID = await _generalService.GetNormalizedID(inventory.Product_ID);
   
                SqlParameter[] parameters = [
                new SqlParameter("@Store_ID", SqlDbType.Int) { Value = inventory.StoreID },
                new SqlParameter("@Product_ID", SqlDbType.VarChar) { Value = normalizedID },
                new SqlParameter("@Location", SqlDbType.VarChar) { Value = inventory.Location },
                new SqlParameter("@Units", SqlDbType.Int) { Value = inventory.Units },
                new SqlParameter("@InspectedBy", SqlDbType.VarChar) { Value = "admin" },
                new SqlParameter("@InspectedDate", SqlDbType.DateTime) { Value = DateTime.Now },
            ];

                int affectedRows = await ExecuteNonQueryAsync(query, parameters);
                if (affectedRows > 0)
                {
                    return Ok(new { message = "Insert successful." });
                }
                return BadRequest(new { message = "Insert failed." });
        }

        [HttpPost("insertStorageLocation")]
        public async Task<IActionResult> InsertStorageLocation(StorageLocation storageLocation)
        {
            string query = "INSERT INTO dbo.StorageLocation(ProductID,StatusID,LatestDate,StoreID,Location,LocationType,Active) " +
                           "VALUES (@ProductID,@StatusID,@Date,@StoreID,@Location,@LocationType,1)";

            var normalizedID = await _generalService.GetNormalizedID(storageLocation.ProductID);
 
                SqlParameter[] parameters = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = normalizedID },
                new SqlParameter("@StatusID", SqlDbType.Int) { Value = storageLocation.StatusID },
                new SqlParameter("@Date", SqlDbType.DateTime) {Value = DateTime.Now },
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = storageLocation.StoreID },
                new SqlParameter("@Location", SqlDbType.VarChar) { Value = storageLocation.Location },
                new SqlParameter("@LocationType", SqlDbType.VarChar) { Value = storageLocation.LocationType },
            
            ];

                int affectedRows = await ExecuteNonQueryAsync(query, parameters);
                if (affectedRows > 0)
                {
                    return Ok(new { message = "Insert successful." });
                }
                return BadRequest(new { message = "Insert failed." });
            
            

        }

        [HttpPost("insertReturnQuantity")]
        private async Task<IActionResult> InsertReturnQuantity(InventoryReturn inventory)
        {
            string query = "INSERT INTO dbo.StorageReturn(ProductID,SupplierID,ReturnQuantity,StoreID,Date,StatusID,UnitCost,Tax,ProductName) " +
                           "VALUES (@ProductID,@SupplierID,@ReturnQuantity,@StoreID,@Date,@StatusID,@UnitCost,@Tax,@ProductName)";

            var normalizedID = await _generalService.GetNormalizedID(inventory.ProductID);

                SqlParameter[] parameters = [
                    new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = normalizedID },
                    new SqlParameter("@SupplierID", SqlDbType.Int) { Value = inventory.SupplierID },
                    new SqlParameter("@ReturnQuantity", SqlDbType.Int) { Value = inventory.ReturnQuantity },
                    new SqlParameter("@StoreID", SqlDbType.Int) { Value = inventory.StoreID },
                    new SqlParameter("@Date", SqlDbType.DateTime) { Value = DateTime.Now },
                    new SqlParameter("@StatusID", SqlDbType.Int) { Value = 0 },
                    new SqlParameter("@UnitCost", SqlDbType.Float) { Value = inventory.UnitCost },
                    new SqlParameter("@Tax", SqlDbType.Int) { Value = inventory.Tax },
                    new SqlParameter("@ProductName", SqlDbType.VarChar) { Value = inventory.ProductName }
                ];

                int affectedRows = await ExecuteNonQueryAsync(query, parameters);
                if (affectedRows > 0)
                {
                    return Ok(new { message = "Insert successful." });
                }
                return BadRequest(new { message = "Insert failed." });
           
        }


        [HttpPost("updateReturnQuantity")]
        private async Task<IActionResult> UpdateReturnQuantity(InventoryReturn inventory)
        {
            string query = "UPDATE dbo.StorageReturn SET ReturnQuantity = @ReturnQuantity, SupplierID = @SupplierID, Date = @Date, " +
                "UnitCost = @UnitCost, Tax = @Tax, ProductName = @ProductName " +
                "WHERE ReturnID = @ReturnID;";

            SqlParameter[] parameters = [
                    new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = inventory.ProductID },
                    new SqlParameter("@ReturnQuantity", SqlDbType.Int) { Value = inventory.ReturnQuantity },
                    new SqlParameter("@SupplierID", SqlDbType.Int) { Value = inventory.SupplierID },
                    new SqlParameter("@Date", SqlDbType.DateTime) { Value = DateTime.Now },
                    new SqlParameter("@ReturnID", SqlDbType.Int) { Value = inventory.ReturnID },
                    new SqlParameter("@UnitCost", SqlDbType.Float) { Value = inventory.UnitCost },
                    new SqlParameter("@Tax", SqlDbType.Int) { Value = inventory.Tax },
                    new SqlParameter("@ProductName", SqlDbType.VarChar) { Value = inventory.ProductName }
                ];

            int affectedRows = await ExecuteNonQueryAsync(query, parameters);
            if (affectedRows > 0)
            {
                return Ok(new { message = "Update successful." });
            }
            return BadRequest(new { message = "Update failed." });
        }

        [HttpPost("updateStorageLocation")]
        private async Task<IActionResult> UpdateStorageLocation(StorageLocation storageLocation)
        {
            string query = "UPDATE dbo.StorageLocation SET StatusID = @StatusID, LatestDate = @LatestDate " +
                "WHERE ProductID = @ProductID AND Location = @Location AND LocationType = @LocationType AND StoreID = @StoreID;";

            var normalizedID = await _generalService.GetNormalizedID(storageLocation.ProductID);
       
                SqlParameter[] parameters = [
                    new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = normalizedID },
                    new SqlParameter("@StatusID", SqlDbType.Int) { Value = storageLocation.StatusID },
                    new SqlParameter("@StoreID", SqlDbType.Int) { Value = storageLocation.StoreID },
                    new SqlParameter("@LatestDate", SqlDbType.DateTime) { Value = DateTime.Now },
                    new SqlParameter("@Location", SqlDbType.VarChar) { Value = storageLocation.Location },
                    new SqlParameter("@LocationType", SqlDbType.VarChar) { Value = storageLocation.LocationType }
                ];

                int affectedRows = await ExecuteNonQueryAsync(query, parameters);
                if (affectedRows > 0)
                {
                    return Ok(new { message = "Update successful." });
                }
                return BadRequest(new { message = "Update failed." });
        }

        [HttpPost("generateCreditNote")]
        public async Task<IActionResult> GenerateCreditNote(List<int> returnIDs, int StoreID)
        {
            if (returnIDs == null || returnIDs.Count == 0)
            {
                return BadRequest(new { message = "No returnIDs provided." });
            }

            // Generate parameterized query
            List<string> parameterNames = new List<string>();
            SqlParameter[] parameters = new SqlParameter[returnIDs.Count + 2];

    
            for (int i = 0; i < returnIDs.Count; i++)
            {
                string paramName = $"@returnID{i}";
                parameterNames.Add(paramName);
                parameters[i] = new SqlParameter(paramName, returnIDs[i]);
            }

            parameters[returnIDs.Count] = new SqlParameter("@PrintDate", DateTime.Now);
            parameters[returnIDs.Count + 1] = new SqlParameter("@CreditNumber", $"CN{StoreID}{DateTime.Now:yyyyMMddHHmmss}");

            string inClause = string.Join(", ", parameterNames);
            string query = $"UPDATE StorageReturn SET StatusID = 1, PrintDate = @PrintDate, CreditNumber = @CreditNumber WHERE returnID IN ({inClause})";

            try
            {
                int affectedRows = await ExecuteNonQueryAsync(query, parameters);
                if (affectedRows > 0)
                {
                    return Ok(new { message = "Update successful." });
                }
                return BadRequest(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the products.", error = ex.Message });
            }
        }

        [HttpPost("isInventoryExist")]
        public async Task<bool> IsInventoryExist(InventoryReturn inventory)
        {
            string query = "SELECT COUNT(1) FROM dbo.StorageReturn " +
                "WHERE ProductID = @ProductID AND SupplierID = @SupplierID AND StoreID = @StoreID AND StatusID = 0";
           
            SqlParameter[] parameters = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = inventory.ProductID },
                new SqlParameter("@SupplierID", SqlDbType.Int) { Value = inventory.SupplierID },
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = inventory.StoreID }
            ];

            object result = await ExecuteScalarAsync(query, parameters);
            return (int)result > 0;
        }

        [HttpPost("isStorageLocationExist")]
        public async Task<bool> IsStorageLocationExist(StorageLocation storageLocation)
        {
            string query = "SELECT COUNT(1) FROM dbo.StorageLocation " +
                "WHERE ProductID = @ProductID AND Location = @Location AND LocationType = @LocationType AND StoreID = @StoreID;";

            SqlParameter[] parameters = [
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = storageLocation.ProductID },
                new SqlParameter("@LocationType", SqlDbType.VarChar) { Value = storageLocation.LocationType },
                new SqlParameter("@Location", SqlDbType.VarChar) { Value = storageLocation.Location },
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = storageLocation.StoreID }
            ];

            object result = await ExecuteScalarAsync(query, parameters);
            return (int)result > 0;
        }

        [HttpPost("submitReturnItem")]
        public async Task<IActionResult> SubmitReturnItem(InventoryReturn inventory)
        {
         
                inventory.ProductID = await _generalService.GetNormalizedID(inventory.ProductID);

                bool exists = await IsInventoryExist(inventory);
                if (exists)
                {
                    return await UpdateReturnQuantity(inventory);
                }
                else
                {
                    return await InsertReturnQuantity(inventory);
                }
           

        }

        [HttpPost("submitStorageLocation")]
        public async Task<IActionResult> SubmitStorageLocation(StorageLocation storageLocation)
        {

                storageLocation.ProductID = await _generalService.GetNormalizedID(storageLocation.ProductID);

                bool exists = await IsStorageLocationExist(storageLocation);
                if (exists)
                {
                    return await UpdateStorageLocation(storageLocation);
                }
                else
                {
                    return await InsertStorageLocation(storageLocation);
                }
           
        }

        /*[HttpPost("deleteReturnItem")]
        public async Task<IActionResult> DeleteReturnItem(int ReturnID)
        {
            string query = "DELETE FROM dbo.StorageReturn " +
                "WHERE ReturnID = @ReturnID;";

            SqlParameter[] parameters = {
                new SqlParameter("@ReturnID", SqlDbType.Int) { Value = ReturnID },
            };

            int affectedRows = await ExecuteNonQueryAsync(query, parameters);
            if (affectedRows > 0)
            {
                return Ok(new { message = "Delete successful." });
            }
            return BadRequest(new { message = "Delete failed." });
        }*/



        private async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[] parameters)
        {
            DataTable table = new();
            using (SqlConnection myCon = new(sqlDataSource))
            {
                await myCon.OpenAsync();
                using (SqlCommand myCommand = new(query, myCon))
                {
                    if (parameters != null)
                    {
                        myCommand.Parameters.AddRange(parameters);
                    }
                    using SqlDataReader myReader = await myCommand.ExecuteReaderAsync();
                    table.Load(myReader);
                }
            }
            return table;
        }

        private async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] parameters)
        {
            using SqlConnection myCon = new(sqlDataSource);
            await myCon.OpenAsync();
            using SqlCommand myCommand = new(query, myCon)
            {
                CommandType = CommandType.Text
            };
            if (parameters != null)
            {
                myCommand.Parameters.AddRange(parameters);
            }
            return await myCommand.ExecuteNonQueryAsync();
        }

        private async Task<object> ExecuteScalarAsync(string query, SqlParameter[] parameters)
        {
            using (SqlConnection myCon = new(sqlDataSource))
            {
                await myCon.OpenAsync();
                using (SqlCommand myCommand = new(query, myCon))
                {
                    if (parameters != null)
                    {
                        myCommand.Parameters.AddRange(parameters);
                    }
                    return await myCommand.ExecuteScalarAsync();
                }
            }
        }

        /* [HttpGet("getNormalizedID")]
         public async Task<IActionResult> GetNormalizedID(string ProductID)
         {
             try
             {
                 var normalizedID = await CallStoredProcedureAsync("Normalize_Barcode", ProductID);
                 return Ok(new { normalizedID = normalizedID });
             }
             catch (Exception)
             {
                 return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
             }
         }

         private async Task<string> CallStoredProcedureAsync(string storedProcedure, string productID)
         {
             using SqlConnection myCon = new SqlConnection(sqlDataSource);
             await myCon.OpenAsync();
             using SqlCommand mycmd = new SqlCommand(storedProcedure, myCon)
             {
                 CommandType = CommandType.StoredProcedure
             };

             // Add the input parameter for the barcode
             mycmd.Parameters.Add(new SqlParameter("@Barcode", SqlDbType.NVarChar, 15) { Value = productID });

             // Add the output parameter for the normalized barcode
             SqlParameter outputParam = new SqlParameter("@Normalized_Barcode", SqlDbType.NVarChar, 15)
             {
                 Direction = ParameterDirection.Output
             };
             mycmd.Parameters.Add(outputParam);

             await mycmd.ExecuteNonQueryAsync();

             // Retrieve the value of the output parameter
             string normalizedBarcode = outputParam.Value.ToString();
             return normalizedBarcode;
         }*/


        [HttpGet("getIfLocationExist")]
        public IActionResult GetIfLocationExist(string location, int storeID)
        {
            var isExist = _context.StoreFloorLocations.Any(x => x.Location == location && x.StoreID == storeID);
            return Ok(new { exists = isExist });
        }



    }
}
