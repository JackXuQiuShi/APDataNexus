using APWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace APWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly string sqlDataSource = configuration.GetConnectionString("pris23Con");


        [HttpGet("getItemByProductID")]
        public async Task<JsonResult> GetItemByProductID(string ProductID)
        {
            string query = "SELECT * FROM dbo.Products WHERE Prod_Num LIKE @ProductID";
            SqlParameter[] parameters = { new SqlParameter("@ProductID", "%" + ProductID + "%") };
            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpGet("getItemByProductName")]
        public async Task<JsonResult> GetItemByProductName(string ProductName)
        {
            string query = "SELECT * FROM dbo.Products WHERE Prod_Name LIKE @ProductName";
            SqlParameter[] parameters = { new SqlParameter("@ProductName", "%" + ProductName + "%") };
            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpGet("getCurrentCost")]
        public async Task<float> GetCurrentCost(string ProductID)
        {
            string query = "SELECT [order] FROM dbo.prices WHERE prodnum = @ProductID";
            SqlParameter[] parameters = { new SqlParameter("@ProductID", ProductID) };

            object result = await ExecuteScalarAsync(query, parameters);
            if (result != null && float.TryParse(result.ToString(), out float currentCost))
            {
                return currentCost;
            }
            return 0;
        }

        [HttpGet("getItemRequest")]
        public async Task<JsonResult> GetItemRequest(int? RequestStoreID, int StatusID)
        {
            string query;
            SqlParameter[] parameters;

            if (RequestStoreID.HasValue)
            {
                query = "SELECT ProductRequest.*,Suppliers.CompanyName, Stores.StoreName, Employees.FullName AS BuyerName FROM dbo.ProductRequest " +
                "LEFT JOIN Suppliers ON ProductRequest.SupplierID = Suppliers.Supplier_ID " +
                "LEFT JOIN Stores ON ProductRequest.RequestStoreID = Stores.Store_ID " +
                "LEFT JOIN Employees ON ProductRequest.BuyerID = Employees.EmployeeID " +
                "WHERE StatusID = @StatusID AND RequestStoreID = @RequestStoreID " +
                "ORDER BY RequestDate DESC;";

                parameters = new SqlParameter[]
                {
                    new("@StatusID", StatusID),
                    new("@RequestStoreID", RequestStoreID.Value)
                };
            }
            else
            {
                query = "SELECT ProductRequest.*,Suppliers.CompanyName, Stores.StoreName, Employees.FullName AS BuyerName FROM dbo.ProductRequest " +
                "LEFT JOIN Suppliers ON ProductRequest.SupplierID = Suppliers.Supplier_ID " +
                "LEFT JOIN Stores ON ProductRequest.RequestStoreID = Stores.Store_ID " +
                "LEFT JOIN Employees ON ProductRequest.BuyerID = Employees.EmployeeID " +
                "WHERE StatusID = @StatusID " +
                "ORDER BY RequestDate DESC;";

                parameters = new SqlParameter[] { new("@StatusID", StatusID) };
            }


            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpGet("getItemBuyer")]
        public async Task<JsonResult> GetItemBuyer(int? StoreID)
        {
            string query;
            DataTable result;
            if (StoreID.HasValue)
            {
                query = "SELECT DepartmentManagement.EmployeeID AS BuyerID, Employees.FullName FROM DepartmentManagement " +
                "LEFT JOIN Employees ON DepartmentManagement.EmployeeID = Employees.EmployeeID " +
                "WHERE StoreID = @StoreID";
                SqlParameter[] parameters = { new SqlParameter("@StoreID", StoreID) };
                result = await ExecuteQueryAsync(query, parameters);
            }
            else {
                query = "SELECT DepartmentManagement.EmployeeID AS BuyerID, Employees.FullName FROM DepartmentManagement " +
                "LEFT JOIN Employees ON DepartmentManagement.EmployeeID = Employees.EmployeeID;";
                result = await ExecuteQueryAsync(query, null);
            }
            
            return new JsonResult(result);
        }

        [HttpGet("isItemExist")]
        public async Task<bool> IsItemExist(string ProductID, int RequestStoreID)
        {
            string queryProductPrice = "SELECT COUNT(1) FROM dbo.ProductPrice WHERE ProdNum = @ProductID AND Store_ID = @RequestStoreID";
            string queryProductsRequest = "SELECT COUNT(1) FROM dbo.ProductRequest WHERE ProductID = @ProductID";

            SqlParameter[] parametersProductPrice = {
                new SqlParameter("@ProductID", ProductID),
                new SqlParameter("@RequestStoreID", RequestStoreID)
            };

            SqlParameter[] parametersProductsRequest = {
                new SqlParameter("@ProductID", ProductID)
            };

            object resultProductsRequest = await ExecuteScalarAsync(queryProductsRequest, parametersProductsRequest);
            if ((int)resultProductsRequest > 0)
            {
                return true;
            }

            object resultProductPrice = await ExecuteScalarAsync(queryProductPrice, parametersProductPrice);
            return (int)resultProductPrice > 0;
        }


        [HttpPost("approveNewProducts")]
        public async Task<int> ApproveNewProducts(List<string> myProductID)
        {
            DataTable productIDsTable = new DataTable();
            productIDsTable.Columns.Add("ProductID", typeof(string));

            foreach (string id in myProductID)
            {
                productIDsTable.Rows.Add(id);
            }

            try
            {
                int totalAffectedRows = 0;
                await ExportNewItemText(myProductID);
                totalAffectedRows += await InsertBystoredProcedureAsync("ProductNewSyncToPOS", productIDsTable);
                totalAffectedRows += await InsertBystoredProcedureAsync("AppendNewProducts", productIDsTable);
                return totalAffectedRows;
            }
            catch (SqlException)
            {
                return 0;
            }
        }

        [HttpPost("rejectNewProducts")]
        public async Task<IActionResult> RejectNewProducts(List<string> myProductID)
        {
            if (myProductID == null || myProductID.Count == 0)
            {
                return BadRequest(new { message = "No product IDs provided." });
            }

            // Create a list of parameter names
            List<string> parameterNames = new List<string>();
            for (int i = 0; i < myProductID.Count; i++)
            {
                parameterNames.Add($"@ProductID{i}");
            }

            // Join the parameter names to form the IN clause
            string inClause = string.Join(", ", parameterNames);
            string query = $"UPDATE ProductRequest SET StatusID = -1 WHERE ProductID IN ({inClause})";

            // Create the SQL parameters
            SqlParameter[] parameters = new SqlParameter[myProductID.Count];
            for (int i = 0; i < myProductID.Count; i++)
            {
                parameters[i] = new SqlParameter($"@ProductID{i}", myProductID[i]);
            }

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


        [HttpPost("insertItemRequest")]
        public async Task<IActionResult> InsertItemRequest(Item item)
        {
            string query = "INSERT INTO dbo.ProductRequest(RequestStoreID, Applicant, DepartmentID, ProductID, CheckDigit, ProductFullName, ProductName, ProductAlias, PackageSpec, Measure, NumPerPack, Tax1App, Tax2App, UnitCost, RetailPrice, PromotionPrice, CaseCost, VolumeUnitCost, MinBulkVolume, CountryOfOrigin, Ethnic, SupplierID, StatusID, UnitSize, UnitSizeUom, BuyerID) " +
                           "VALUES (@RequestStoreID, @Applicant, @DepartmentID, @ProductID, @CheckDigit, @ProductFullName, @ProductName, @ProductAlias, @PackageSpec, @Measure, @NumPerPack, @Tax1App, @Tax2App, @UnitCost, @RetailPrice, @PromotionPrice, @CaseCost, @VolumeUnitCost, @MinBulkVolume, @CountryOfOrigin, @Ethnic, @SupplierID, @StatusID, @UnitSize, @UnitSizeUom, @BuyerID)";

            SqlParameter[] parameters = {

                new SqlParameter("@RequestStoreID", SqlDbType.Int) { Value = item.RequestStoreID },
                new SqlParameter("@Applicant", SqlDbType.VarChar) { Value = item.Applicant },
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = item.DepartmentID },
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = item.ProductID },
                new SqlParameter("@CheckDigit", SqlDbType.Int) { Value = item.CheckDigit },
                new SqlParameter("@ProductFullName", SqlDbType.VarChar) { Value = item.ProductFullName },
                new SqlParameter("@ProductName", SqlDbType.VarChar) { Value = item.ProductName },
                new SqlParameter("@ProductAlias", SqlDbType.NVarChar) { Value = item.ProductAlias },
                new SqlParameter("@PackageSpec", SqlDbType.VarChar) { Value = item.PackageSpec },
                new SqlParameter("@Measure", SqlDbType.VarChar) { Value = item.Measure },
                new SqlParameter("@NumPerPack", SqlDbType.Float) { Value = item.NumPerPack },
                new SqlParameter("@Tax1App", SqlDbType.Int) { Value = item.Tax1App },
                new SqlParameter("@Tax2App", SqlDbType.Int) { Value = item.Tax2App },
                new SqlParameter("@UnitCost", SqlDbType.Float) { Value = item.UnitCost },
                new SqlParameter("@RetailPrice", SqlDbType.Float) { Value = item.RetailPrice },
                new SqlParameter("@PromotionPrice", SqlDbType.Float) { Value = item.PromotionPrice },
                new SqlParameter("@CaseCost", SqlDbType.Float) { Value = item.UnitCost * item.NumPerPack },
                new SqlParameter("@VolumeUnitCost", SqlDbType.Float) { Value = item.VolumeUnitCost },
                new SqlParameter("@MinBulkVolume", SqlDbType.Float) { Value = item.MinBulkVolume },
                new SqlParameter("@CountryOfOrigin", SqlDbType.VarChar) { Value = item.CountryOfOrigin },
                new SqlParameter("@Ethnic", SqlDbType.VarChar) { Value = item.Ethnic },
                new SqlParameter("@SupplierID", SqlDbType.Int) { Value = item.SupplierID },
                new SqlParameter("@StatusID", SqlDbType.Int) { Value = 0 },
                new SqlParameter("@UnitSize", SqlDbType.Float) { Value = item.UnitSize },
                new SqlParameter("@UnitSizeUom", SqlDbType.VarChar) { Value = item.UnitSizeUom },
                new SqlParameter("@BuyerID", SqlDbType.VarChar) { Value = item.BuyerID }
            }; 

            int affectedRows = await ExecuteNonQueryAsync(query, parameters);
            if (affectedRows > 0)
            {
                return Ok(new { message = "Insert successful." });
            }
            return BadRequest(new { message = "Insert failed." });
        }



        [HttpPost("insertItemUpdateRequest")]
        public async Task<IActionResult> InsertItemUpdateRequest(Item item)
        {
            string query = "INSERT INTO dbo.ProductUpdateRequest(RequestDate, RequestStoreID, Applicant, ProductID, UnitCost, CurrentUnitCost) " +
                           "VALUES (@RequestDate, @RequestStoreID, @Applicant, @ProductID, @UnitCost, @CurrentUnitCost)";

            SqlParameter[] parameters = {
                new SqlParameter("@RequestDate", SqlDbType.DateTime) { Value = DateTime.Now },
                new SqlParameter("@RequestStoreID", SqlDbType.Int) { Value = item.RequestStoreID },
                new SqlParameter("@Applicant", SqlDbType.VarChar) { Value = item.Applicant },
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = item.ProductID },
                new SqlParameter("@UnitCost", SqlDbType.Float) { Value = item.UnitCost },
                new SqlParameter("@CurrentUnitCost", SqlDbType.Float) { Value = await GetCurrentCost(item.ProductID) }
            };

            int affectedRows = await ExecuteNonQueryAsync(query, parameters);
            if (affectedRows > 0)
            {
                return Ok(new { message = "Insert successful." });
            }
            return BadRequest(new { message = "Insert failed." });
        }

        [HttpPost("deleteItemDraft")]
        public async Task<IActionResult> DeleteItemDraft(string ProductID)
        {
            string query = "DELETE FROM dbo.ProductRequest " +
                "WHERE ProductID = @ProductID;";

            SqlParameter[] parameters = {
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
            };

            int affectedRows = await ExecuteNonQueryAsync(query, parameters);
            if (affectedRows > 0)
            {
                return Ok(new { message = "Delete successful." });
            }
            return BadRequest(new { message = "Delete failed." });
        }

        [HttpPost("submitItemDraft")]
        public async Task<IActionResult> SubmitItemDraft(List<string> myProductID)
        {
         
            if (myProductID == null || myProductID.Count == 0)
            {
                return BadRequest(new { message = "No product IDs provided." });
            }

            // Create a list of parameter names
            List<string> parameterNames = new List<string>();
            for (int i = 0; i < myProductID.Count; i++)
            {
                parameterNames.Add($"@ProductID{i}");
            }

            // Join the parameter names to form the IN clause
            string inClause = string.Join(", ", parameterNames);
          
            string query = $"UPDATE ProductRequest SET StatusID = 1, RequestDate = @RequestDate " +
                $"WHERE ProductID IN ({inClause})";

   
            // Create the SQL parameters
            SqlParameter[] parameters = new SqlParameter[myProductID.Count + 1];
            for (int i = 0; i < myProductID.Count; i++)
            {
                parameters[i] = new SqlParameter($"@ProductID{i}", myProductID[i]);
            }
            parameters[myProductID.Count] = new SqlParameter($"@RequestDate", DateTime.Now);

            try
            {
                int affectedRows = await ExecuteNonQueryAsync(query, parameters);
                if (affectedRows > 0)
                {
                    return Ok(new { message = "Submit successful." });
                }
                return BadRequest(new { message = "Submit failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while submit the products.", error = ex.Message });
            }

        }

        [HttpPost("updateItemDraft")]
        public async Task<IActionResult> UpdateItemDraft(Item item)
        {
            string query = "UPDATE ProductRequest SET DepartmentID = @DepartmentID, " +
                "CheckDigit = @CheckDigit, " +
                "ProductFullName = @ProductFullName, " +
                "ProductName = @ProductName, " +
                "ProductAlias = @ProductAlias, " +
                "PackageSpec = @PackageSpec, " +
                "Measure = @Measure, " +
                "NumPerPack = @NumPerPack, " +
                "Tax1App = @Tax1App, " +
                "Tax2App = @Tax2App, " +
                "UnitCost = @UnitCost, " +
                "RetailPrice = @RetailPrice, " +
                "PromotionPrice = @PromotionPrice, " +
                "VolumeUnitCost = @VolumeUnitCost, " +
                "MinBulkVolume = @MinBulkVolume, " +
                "CountryOfOrigin = @CountryOfOrigin, " +
                "Ethnic = @Ethnic, " +
                "SupplierID = @SupplierID, " +
                "UnitSize = @UnitSize, " +
                "UnitSizeUom = @UnitSizeUom, " +
                "BuyerID = @BuyerID, " +
                "CaseCost = @CaseCost, " +
                "StatusID = @StatusID " +
                "WHERE ProductID = @ProductID;";

            SqlParameter[] parameters = {
                    new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = item.DepartmentID },
                    new SqlParameter("@CheckDigit", SqlDbType.Int) { Value = item.CheckDigit },
                    new SqlParameter("@ProductFullName", SqlDbType.VarChar) { Value = item.ProductFullName },
                    new SqlParameter("@ProductName", SqlDbType.VarChar) { Value = item.ProductName },
                    new SqlParameter("@ProductAlias", SqlDbType.NVarChar) { Value = item.ProductAlias },
                    new SqlParameter("@PackageSpec", SqlDbType.VarChar) { Value = item.PackageSpec },
                    new SqlParameter("@Measure", SqlDbType.VarChar) { Value = item.Measure },
                    new SqlParameter("@NumPerPack", SqlDbType.Float) { Value = item.NumPerPack },
                    new SqlParameter("@Tax1App", SqlDbType.Int) { Value = item.Tax1App },
                    new SqlParameter("@Tax2App", SqlDbType.Int) { Value = item.Tax2App },
                    new SqlParameter("@UnitCost", SqlDbType.Float) { Value = item.UnitCost },
                    new SqlParameter("@RetailPrice", SqlDbType.Float) { Value = item.RetailPrice },
                    new SqlParameter("@PromotionPrice", SqlDbType.Float) { Value = item.PromotionPrice },
                    new SqlParameter("@VolumeUnitCost", SqlDbType.Float) { Value = item.VolumeUnitCost },
                    new SqlParameter("@MinBulkVolume", SqlDbType.Float) { Value = item.MinBulkVolume },
                    new SqlParameter("@CountryOfOrigin", SqlDbType.VarChar) { Value = item.CountryOfOrigin },
                    new SqlParameter("@Ethnic", SqlDbType.VarChar) { Value = item.Ethnic },
                    new SqlParameter("@SupplierID", SqlDbType.Int) { Value = item.SupplierID },
                    new SqlParameter("@UnitSize", SqlDbType.Float) { Value = item.UnitSize },
                    new SqlParameter("@UnitSizeUom", SqlDbType.VarChar) { Value = item.UnitSizeUom },
                    new SqlParameter("@BuyerID", SqlDbType.Int) { Value = item.BuyerID },
                    new SqlParameter("@CaseCost", SqlDbType.Float) { Value = item.UnitCost * item.NumPerPack },
                    new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = item.ProductID },
                    new SqlParameter("@StatusID", SqlDbType.Int) { Value = 0 }
                };

            int affectedRows = await ExecuteNonQueryAsync(query, parameters);
            if (affectedRows > 0)
            {
                return Ok(new { message = "Update successful." });
            }
            return BadRequest(new { message = "Update failed." });
        }

        [HttpPost("updateItemPrice")]
        public async Task<IActionResult> UpdateItemPrice(List<string> myProductID)
        {
            if (myProductID == null || myProductID.Count == 0)
            {
                return BadRequest(new { message = "No product IDs provided." });
            }
            // Create a list of parameter names
            List<string> parameterNames = new List<string>();
            for (int i = 0; i < myProductID.Count; i++)
            {
                parameterNames.Add($"@ProductID{i}");
            }

            string inClause = string.Join(", ", parameterNames);
            string query = "UPDATE prices SET [order] = " +
            "(SELECT UnitCost FROM ProductUpdateRequest WHERE ProductUpdateRequest.ProductID = prices.prodnum) " +
            $"WHERE ProductID IN ({inClause})";

            SqlParameter[] parameters = new SqlParameter[myProductID.Count];
            for (int i = 0; i < myProductID.Count; i++)
            {
                parameters[i] = new SqlParameter($"@ProductID{i}", myProductID[i]);
            }

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

        [HttpPost("exportNewItemText")]
        public async Task<IActionResult> ExportNewItemText(List<string> myProductID)
        {
            if (myProductID == null || myProductID.Count == 0)
            {
                return BadRequest(new { message = "No product IDs provided." });
            }

            // Create a list of parameter names
            List<string> parameterNames = new List<string>();
            for (int i = 0; i < myProductID.Count; i++)
            {
                parameterNames.Add($"@ProductID{i}");
            }

            // Join the parameter names to form the IN clause
            string inClause = string.Join(", ", parameterNames);
            string queryTemplate = $"SELECT '' as PriceGroupName, ProductID, ProductName, CountryOfOrigin, Measure, 0 as OnSales, " +
                "ProductAlias, Tax1App, Tax2App, PackageSpec, CASE WHEN Measure = 'EA' THEN 0 ELSE 1 END AS PriceMode, 0 as QtySale, 0 as QtySaleQty, 0 as QtySalePrice, 0 as ScaleTray, " +
                "d.DepartmentName, 0 as Inactive, 0 as QtyGroup, 0 as MaxBuyQty, 0 as MaxOnSaleQty, 0 as isFood, CAST(RetailPrice AS NVARCHAR(255)) AS RegPrice,CAST(PromotionPrice AS NVARCHAR(255)) AS PromotionPrice, " +
                "pr.DepartmentID, NumPerPack,CAST(pr.UnitCost AS NVARCHAR(255)) AS UnitCost,CAST(pr.VolumeUnitCost AS NVARCHAR(255)) AS VolumeCost, MinBulkVolume, Ethnic " +
                "FROM ProductRequest pr " +
                "LEFT JOIN Department d ON pr.DepartmentID = d.DepartmentID " +
                "LEFT JOIN ProductPrice pp ON pp.ProdNum = pr.ProductID AND pp.Store_ID = @Store_ID " +
                $"WHERE pr.ProductID IN ({inClause}) AND pp.ProdNum IS NULL;";

            // Define the store IDs and corresponding folder paths
            var storeIDFolderMap = new Dictionary<int, string>
            {
                { 39, "C:\\PRIS23\\POSSync\\1970\\" },
                { 3, "C:\\PRIS23\\POSSync\\888\\" },
                { 7, "C:\\PRIS23\\POSSync\\250\\" }
            };
            List<string> filePaths = new List<string>();

            foreach (var entry in storeIDFolderMap)
            {
                int storeID = entry.Key;
                string folderPath = entry.Value;

                // Ensure the directory exists
                Directory.CreateDirectory(folderPath);

                // Create the SQL parameters
                List<SqlParameter> parameters = new List<SqlParameter>();
                for (int i = 0; i < myProductID.Count; i++)
                {
                    parameters.Add(new SqlParameter($"@ProductID{i}", myProductID[i]));
                }
                parameters.Add(new SqlParameter("@Store_ID", storeID));

                string query = queryTemplate.Replace("@Store_ID", storeID.ToString());

                try
                {
                    DataTable result = await ExecuteQueryAsync(query, parameters.ToArray());
                    string fileName = $"tbl_POS_UpdateList_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                    string outputPath = Path.Combine(folderPath, fileName);
                    ExportDataTableToTextFile(result, outputPath);
                    filePaths.Add(outputPath);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "An error occurred while exporting the products for Store_ID " + storeID, error = ex.Message });
                }
            }

            return Ok(new { message = "Export successful.", filePaths = filePaths });
        }

        private void ExportDataTableToTextFile(DataTable table, string filePath)
        {
            Encoding big5 = Encoding.GetEncoding("Big5");
            using (StreamWriter writer = new StreamWriter(filePath, false, big5))
            {
                // Write column headers
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    writer.Write(table.Columns[i].ColumnName);
                    if (i < table.Columns.Count - 1)
                    {
                        writer.Write("\t");
                    }
                }
                writer.WriteLine();

                // Write rows
                foreach (DataRow row in table.Rows)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        string value = row[i].ToString().Replace("\t", " ").Replace("\n", " ").Replace("\r", " ");
                        writer.Write(value);
                        if (i < table.Columns.Count - 1)
                        {
                            writer.Write("\t");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }

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
                myCommand.CommandTimeout = 120;
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
                        myCommand.CommandTimeout = 120;
                    }
                    return await myCommand.ExecuteScalarAsync();
                }
            }
        }

        private async Task<int> InsertBystoredProcedureAsync(string storedProcedure, DataTable productIDsTable)
        {
            using SqlConnection myCon = new(sqlDataSource);
            await myCon.OpenAsync();
            using SqlCommand mycmd = new(storedProcedure, myCon)
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlParameter tvpParam = mycmd.Parameters.AddWithValue("@ProductIDTable", productIDsTable);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = "dbo.ProductIDTableType";

            return await mycmd.ExecuteNonQueryAsync();
        }



        /*
        [HttpPost("approve")]
        public IActionResult Approve(Item item)
        {
            try
            {
                int totalAffectedRows = 0;

                totalAffectedRows += InsertRequestToProducts(item);
                totalAffectedRows += InsertRequestToPrices(item);
                totalAffectedRows += InsertRequestToProductPrice(item);
                totalAffectedRows += InsertRequestToProductsPris(item);

                if (totalAffectedRows == 4)
                {
                    return Ok(new { message = "Insert successful." });
                }
                else
                {
                    return BadRequest(new { message = "Insert failed." });
                }

            }
            catch (SqlException e)
            {
                // Log the error or do something with it
                return StatusCode(500, new { message = e.Message });
            }
        }


        [HttpPost("insertRequestToProducts")]
        public int InsertRequestToProducts(Item item)
        {
            string query;
            *//*query = "INSERT INTO dbo.Products(Prod_Num, ProductName, Prod_Desc, UnitCost, Measure, ProductAlias, Tax1App, Tax2App, PackageSpec, PriceMode, QtySale, QtySaleQty, QtySalePrice, ModTimeStamp, DepartmentID, Last_Ord_Date, On_Order, Ord_Point, isFood, Source) " +
                    "VALUES (@Prod_Num, @ProductName, @Prod_Desc, @UnitCost, @Measure, @ProductAlias, @Tax1App, @Tax2App, @PackageSpec, @PriceMode, @QtySale, @QtySaleQty, @QtySalePrice, @ModTimeStamp, @DepartmentID, @Last_Ord_Date, @On_Order, @Ord_Point, @isFood, @Source)";*//*

            query = "INSERT INTO dbo.Products(Prod_Num, Prod_Name, Prod_Desc, Unit_Cost, Measure, Prod_Alias, Tax1App, Tax2App, PackageSpec, PriceMode, QtySale, QtySaleQty, QtySalePrice, ModTimeStamp, Department, Last_Ord_Date, On_Order, Ord_Point, isFood, Source) " +
                "SELECT Prod_Num, ProductName, Prod_Desc, UnitCost, Measure, ProductAlias, Tax1App, Tax2App, PackageSpec, @PriceMode, QtySale, QtySaleQty, QtySalePrice, @ModTimeStamp, DepartmentID, @Last_Ord_Date, @On_Order, @Ord_Point, @isFood, @Source " +
                "FROM ProductRequest " +
                "WHERE ProductID = @ProductID";

            string formattedDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            int PriceMode;
            if (item.Measure == "EA") {
                PriceMode = 0;
            }else {
                PriceMode = 1;
            }


            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);
            myCommand.Parameters.Add("@Prod_Num", SqlDbType.VarChar).Value = item.ProductID;
            myCommand.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = item.ProductName;
            myCommand.Parameters.Add("@Prod_Desc", SqlDbType.VarChar).Value = item.CountryOfOrigin;
            myCommand.Parameters.Add("@UnitCost", SqlDbType.Money).Value = item.UnitCost;
            myCommand.Parameters.Add("@Measure", SqlDbType.VarChar).Value = item.Measure;
            myCommand.Parameters.Add("@ProductAlias", SqlDbType.NVarChar).Value = item.ProductAlias;
            myCommand.Parameters.Add("@Tax1App", SqlDbType.SmallInt).Value = item.Tax1App;
            myCommand.Parameters.Add("@Tax2App", SqlDbType.SmallInt).Value = item.Tax2App;
            myCommand.Parameters.Add("@PackageSpec", SqlDbType.VarChar).Value = item.PackageSpec;
            myCommand.Parameters.Add("@PriceMode", SqlDbType.SmallInt).Value = PriceMode;
            myCommand.Parameters.Add("@QtySale", SqlDbType.Int).Value = 0;
            myCommand.Parameters.Add("@QtySaleQty", SqlDbType.Int).Value = 0;
            myCommand.Parameters.Add("@QtySalePrice", SqlDbType.Float).Value = 0;
            myCommand.Parameters.Add("@ModTimeStamp", SqlDbType.VarChar).Value = formattedDateTime;
            myCommand.Parameters.Add("@DepartmentID", SqlDbType.VarChar).Value = item.DepartmentID;
            myCommand.Parameters.Add("@Last_Ord_Date", SqlDbType.VarChar).Value = "N";
            myCommand.Parameters.Add("@On_Order", SqlDbType.Float).Value = 0;
            myCommand.Parameters.Add("@Ord_Point", SqlDbType.Float).Value = 0;
            myCommand.Parameters.Add("@isFood", SqlDbType.SmallInt).Value = 0;
            myCommand.Parameters.Add("@Source", SqlDbType.VarChar).Value = 0;

            try
            {
                int affectedRows = myCommand.ExecuteNonQuery();
                return affectedRows;
            }
            catch (SqlException)
            {
                return 0; // Consider logging the exception or handling it as needed
            }
        }


        [HttpPost("insertRequestToPrices")]
        public int InsertRequestToPrices(Item item)
        {
            string query;
            query = "INSERT INTO dbo.prices(prodnum, [order], created_at, updated_at, updated_by) " +
                    "VALUES (@prodnum, @order, @created_at, @updated_at, @updated_by)";

            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);
            myCommand.Parameters.Add("@prodnum", SqlDbType.VarChar).Value = item.ProductID;
            myCommand.Parameters.Add("@order", SqlDbType.Money).Value = item.UnitCost;
            myCommand.Parameters.Add("@created_at", SqlDbType.VarChar).Value = DateTime.Now;
            myCommand.Parameters.Add("@updated_at", SqlDbType.VarChar).Value = DateTime.Now;
            myCommand.Parameters.Add("@updated_by", SqlDbType.VarChar).Value = item.Applicant;


            try
            {
                int affectedRows = myCommand.ExecuteNonQuery();
                return affectedRows;
            }
            catch (SqlException)
            {
                return 0; // Consider logging the exception or handling it as needed
            }
        }


        [HttpPost("insertRequestToProductPrice")]
        public int InsertRequestToProductPrice(Item item)
        {
            string query;
            query = "INSERT INTO dbo.ProductPrice(ProdNum, Grade, RegPrice, BottomPrice, OnsalePrice, ModTimeStamp, Source, RequestStoreID) " +
                    "VALUES (@ProdNum, @Grade, @RegPrice, @BottomPrice, @OnsalePrice, @ModTimeStamp, @Source, @RequestStoreID)";

            string formattedDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);
            myCommand.Parameters.Add("@ProdNum", SqlDbType.VarChar).Value = item.ProductID;
            myCommand.Parameters.Add("@Grade", SqlDbType.VarChar).Value = "RETAIL";
            myCommand.Parameters.Add("@RegPrice", SqlDbType.Money).Value = item.RetailPrice;
            myCommand.Parameters.Add("@BottomPrice", SqlDbType.Money).Value = 0.00;
            myCommand.Parameters.Add("@OnsalePrice", SqlDbType.Money).Value = item.PromotionPrice;
            myCommand.Parameters.Add("@ModTimeStamp", SqlDbType.VarChar).Value = formattedDateTime;
            myCommand.Parameters.Add("@Source", SqlDbType.VarChar).Value = "WEB";
            myCommand.Parameters.Add("@RequestStoreID", SqlDbType.VarChar).Value = item.RequestStoreID;

            try
            {
                int affectedRows = myCommand.ExecuteNonQuery();
                return affectedRows;
            }
            catch (SqlException)
            {
                return 0; // Consider logging the exception or handling it as needed
            }
        }


        [HttpPost("insertRequestToProductsPris")]
        public int InsertRequestToProductsPris(Item item)
        {
            string query;
            query = "INSERT INTO dbo.Products_Pris(Barcode,Rank,ModTime,InStock,InStockDate,Source,Active,WM_Code) " +
                    "VALUES (@Barcode,@Rank,@ModTime,@InStock,@InStockDate,@Source,@Active,@WM_Code)";

            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);
            myCommand.Parameters.Add("@Barcode", SqlDbType.VarChar).Value = item.ProductID;
            myCommand.Parameters.Add("@Rank", SqlDbType.VarChar).Value = "N";
            myCommand.Parameters.Add("@ModTime", SqlDbType.VarChar).Value = DateTime.Now;
            myCommand.Parameters.Add("@InStock", SqlDbType.SmallInt).Value = 0;
            myCommand.Parameters.Add("@InStockDate", SqlDbType.VarChar).Value = DateTime.Now;
            myCommand.Parameters.Add("@Source", SqlDbType.VarChar).Value = "POS";
            myCommand.Parameters.Add("@Active", SqlDbType.SmallInt).Value = 1;
            myCommand.Parameters.Add("@WM_Code", SqlDbType.Int).Value = 92;
           
            try
            {
                int affectedRows = myCommand.ExecuteNonQuery();
                return affectedRows;
            }
            catch (SqlException)
            {
                return 0; // Consider logging the exception or handling it as needed
            }
        }
*/




        /*
        [HttpPost("InsertItem")]
        public JsonResult InsertItem(Item Item)
        {
            string sqlDataSource = _configuration.GetConnectionString("prisCon");
            DataTable table = new DataTable();
            SqlDataReader myReader;
            string query;


            query = "INSERT INTO dbo.Categories(Item_Id, Item) " +
                "VALUES (@Item_Id, @Item)";


            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.Add("@Item_Id", SqlDbType.Int).Value = Item.Item_Id;
                    myCommand.Parameters.Add("@Item", SqlDbType.VarChar).Value = Item.Item_Name;
                

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }


        [HttpPost("DeleteItemByID")]
        public JsonResult DeleteItemByID(string Item_Id)
        {
            string sqlDataSource = _configuration.GetConnectionString("prisCon");
            DataTable table = new DataTable();
            SqlDataReader myReader;
            string query;


            query = "DELETE FROM dbo.Categories WHERE Item_Id = @Item_Id;";


            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.Add("@Item_Id", SqlDbType.Int).Value = Item_Id;

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }


        [HttpPost("UpdateItemByID")]
        public JsonResult Update(int Item_Id, string Item)
        {
            string sqlDataSource = _configuration.GetConnectionString("prisCon");
            DataTable table = new DataTable();
            SqlDataReader myReader;
            string query;

            query = "UPDATE dbo.Categories SET Item = @Item where Item_Id = @Item_Id;";

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.Add("@Item_Id", SqlDbType.Int).Value = Item_Id;
                    myCommand.Parameters.Add("@Item", SqlDbType.VarChar).Value = Item;
                   

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        */


        /*
        [HttpGet("getItemRequest")]
        public async Task<JsonResult> GetItemRequest()
        {
            string query = "SELECT ProductRequest.*,Suppliers.CompanyName, Stores.StoreName, Employees.FullName AS BuyerName FROM dbo.ProductRequest " +
                "LEFT JOIN Suppliers ON ProductRequest.SupplierID = Suppliers.Supplier_ID " +
                "LEFT JOIN Stores ON ProductRequest.RequestStoreID = Stores.Store_ID " +
                "LEFT JOIN Employees ON ProductRequest.BuyerID = Employees.EmployeeID " +
                "WHERE StatusID = 1";
            DataTable result = await ExecuteQueryAsync(query, null);
            return new JsonResult(result);
        }

        

        [HttpGet("getItemDraft")]
        public async Task<JsonResult> GetItemDraft(int RequestStoreID)
        {
            string query = "SELECT ProductRequest.*,Suppliers.CompanyName, Stores.StoreName, Employees.FullName AS BuyerName FROM dbo.ProductRequest " +
                "LEFT JOIN Suppliers ON ProductRequest.SupplierID = Suppliers.Supplier_ID " +
                "LEFT JOIN Stores ON ProductRequest.RequestStoreID = Stores.Store_ID " +
                "LEFT JOIN Employees ON ProductRequest.BuyerID = Employees.EmployeeID " +
                "WHERE RequestStoreID = @RequestStoreID AND StatusID = 0";
            SqlParameter[] parameters = { new SqlParameter("@RequestStoreID", RequestStoreID) };
            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }*/





    }
}
