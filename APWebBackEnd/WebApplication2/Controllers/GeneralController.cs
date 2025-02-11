using APWeb.Dtos;
using APWeb.Mappers;
using APWeb.Models;
using APWeb.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace APWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController(IConfiguration configuration, IGeneralService generalService, ApplicationDbContext context) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly string sqlDataSource = configuration.GetConnectionString("pris23Con");
        private readonly IGeneralService _generalService = generalService;
        private readonly ApplicationDbContext _context = context;
        

        [HttpGet("getLastPODetails")]
        public async Task<JsonResult> GetLastPODetails(string ProductID, int StoreID)
        {
            string query = "SELECT TOP 1 p.PO_ID,p.Product_ID AS ProductID,p.UnitsOrdered,p.PriceOrdered,p.UnitsReceived,p.PriceReceived,p.UnitsPerPackage,p.TaxRate, p.OrderingDate,p.ReceivingDate, " +
                "po.Supplier_ID AS SupplierID, s.CompanyName " +
                "FROM PO_Details p LEFT JOIN POs po ON p.PO_ID = po.PO_ID " +
                "LEFT JOIN Suppliers s ON s.supplier_id = po.supplier_id " +
                "WHERE p.Store_ID = @StoreID AND p.Product_ID = @ProductID AND not PriceReceived is null " +
                "ORDER BY p.OrderingDate DESC;";

            SqlParameter[] parameters = [
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID },
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpGet("getRegPrice")]
        public async Task<JsonResult> GetRegPrice(string ProductID, int StoreID)
        {
            string query = "SELECT RegPrice FROM ProductPrice " +
                "WHERE ProdNum = @ProductID AND Store_ID = @StoreID";

            SqlParameter[] parameters = [
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID },
                new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }


        [HttpGet("getMaxMinCost")]
        public async Task<IActionResult> GetMaxMinCost(string ProductID)
        {
            // 定义三年前的日期
            DateTime startDate = DateTime.Now.AddYears(-3);

            // 获取最低成本及其 StoreID
            var minCostDetails = await _context.PO_Details
                .Where(p => p.ReceivingDate >= startDate && p.Product_ID == ProductID)
                .OrderBy(p => p.PriceReceived)
                .FirstOrDefaultAsync();

            // 获取最高成本及其 StoreID
            var maxCostDetails = await _context.PO_Details
                .Where(p => p.ReceivingDate >= startDate && p.Product_ID == ProductID)
                .OrderByDescending(p => p.PriceReceived)
                .FirstOrDefaultAsync();

            // 返回最低和最高的 StoreID 和 Cost
            return Ok(new
            {
                MinStoreID = minCostDetails?.Store_ID,
                MinCost = minCostDetails?.PriceReceived,
                MaxStoreID = maxCostDetails?.Store_ID,
                MaxCost = maxCostDetails?.PriceReceived
            });
        }


        [HttpGet("getNormalizedID")]
        public async Task<IActionResult> GetNormalizedID(string ProductID)
        {
            var normalizedIDResult = await _generalService.GetNormalizedID(ProductID);
            return Ok(new { normalizedID = normalizedIDResult });
        }

        //=================Inventory========================================================
        [HttpGet("getProductInspectionResult")]
        public async Task<JsonResult> GetProductInspectionResult(int? SupplierID, int StoreID)
        {
            string query;
            SqlParameter[] parameters;

            if (SupplierID.HasValue)
            {
                query = "SELECT ProductID, StoreID, ProductName, ProductChineseName, Department, LastSalesUnitPrice AS RetailPrice, SupplierID, SupplierName, LastPOReceivedUnitCost AS UnitCost, Tax, LocationName " +
                   "FROM dbo.ProductInspectionResult " +
                   "WHERE StoreID = @StoreID AND SupplierID = @SupplierID AND StatusID = 0 " +
                   "ORDER BY SupplierName, LocationName, ProductID";

                parameters = [
                    new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID },
                    new SqlParameter("@SupplierID", SqlDbType.Int) { Value = SupplierID }
                ];
            }
            else
            {
                query = "SELECT ProductID, StoreID, ProductName, ProductChineseName, Department, LastSalesUnitPrice AS RetailPrice, SupplierID, SupplierName, LastPOReceivedUnitCost AS UnitCost, Tax, LocationName " +
                   "FROM dbo.ProductInspectionResult " +
                   "WHERE StoreID = @StoreID AND StatusID = 0 " +
                   "ORDER BY SupplierName, LocationName, ProductID";

                parameters = [
                    new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID }
                ];
            }


            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpGet("getProductInspectionResultSuppliers")]
        public async Task<JsonResult> GetProductInspectionResultSuppliers(int StoreID)
        {
            string query = "SELECT SupplierID, SupplierName " +
                "FROM dbo.ProductInspectionResult " +
                "WHERE StoreID = @StoreID AND StoreID = @StoreID AND StatusID = 0 " +
                "GROUP BY SupplierID, SupplierName " +
                "ORDER BY SupplierName;";

            SqlParameter[] parameters = [
                new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID }
            ];

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpPost("updateProductInspectionStatus")]
        public async Task<IActionResult> UpdateProductInspectionStatus(InventoryReturn inventory)
        {
            try
            {
                string updateQuery = "UPDATE dbo.ProductInspectionResult SET StatusID = 1 " +
                    "WHERE ProductID = @ProductID AND StoreID = @StoreID;";

                SqlParameter[] updateParameters = {
                    new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = inventory.ProductID },
                    new SqlParameter("@StoreID", SqlDbType.Int) { Value = inventory.StoreID }
                };

                int affectedRows = await ExecuteNonQueryAsync(updateQuery, updateParameters);
                if (affectedRows > 0)
                {
                    return Ok(new { message = "Update successful." });
                }
                return NotFound(new { message = "Product does not exist or update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        //=========================================================================










        /*[HttpGet("getLastPOCost")]
        public async Task<JsonResult> GetLastPOCost(string ProductID, int? StoreID)
        {
            string query;
            SqlParameter[] parameters;

            if (StoreID.HasValue)
            {
                query = "SELECT TOP 1 PriceReceived, Store_ID " +
                "FROM PO_Details " +
                "WHERE Product_ID = @ProductID AND Store_ID = @StoreID " +
                "ORDER BY ReceivingDate DESC;";

                parameters = [
                    new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
                    new SqlParameter("@StoreID", SqlDbType.Int) { Value = StoreID }
                ];
            }
            else
            {
                query = "SELECT TOP 1 PriceReceived, Store_ID " +
                "FROM PO_Details " +
                "WHERE Product_ID = @ProductID " +
                "ORDER BY ReceivingDate DESC;";

                parameters = [
                    new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID }
                ];
            }

            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }*/


        /*   [HttpGet("getInfoFromPOS306090")]
           public async Task<JsonResult> GetInfoFromPOS306090(string ProductID, int StoreID)
           {
               DateTime today = DateTime.Today;
               DateTime startDate30 = today.AddDays(-30);
               DateTime startDate60 = today.AddDays(-60);
               DateTime startDate90 = today.AddDays(-90);

               string query30 = "SELECT SUM(Quantity) AS Total_QTY_30 FROM POS_Sales " +
                   "WHERE Product_ID = @ProductID AND Store_ID = @StoreID AND Date >= @startDate30 AND Date < @today " +
                   "GROUP BY Product_ID";

               string query60 = "SELECT SUM(Quantity) AS Total_QTY_60 FROM POS_Sales " +
                   "WHERE Product_ID = @ProductID AND Store_ID = @StoreID AND Date >= @startDate60 AND Date < @startDate30 " +
                   "GROUP BY Product_ID";

               string query90 = "SELECT SUM(Quantity) AS Total_QTY_90 FROM POS_Sales " +
                   "WHERE Product_ID = @ProductID AND Store_ID = @StoreID AND Date >= @startDate90 AND Date < @startDate60 " +
                   "GROUP BY Product_ID";

               SqlParameter[] parameters30 = [
                   new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
                   new SqlParameter("@StoreID", SqlDbType.VarChar) { Value = StoreID },
                   new SqlParameter("@startDate30", SqlDbType.DateTime) { Value = startDate30 },
                   new SqlParameter("@today", SqlDbType.DateTime) { Value = today }
               ];

               SqlParameter[] parameters60 = [
                   new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
                   new SqlParameter("@StoreID", SqlDbType.VarChar) { Value = StoreID },
                   new SqlParameter("@startDate60", SqlDbType.DateTime) { Value = startDate60 },
                   new SqlParameter("@startDate30", SqlDbType.DateTime) { Value = startDate30 }
               ];

               SqlParameter[] parameters90 = [
                   new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID },
                   new SqlParameter("@StoreID", SqlDbType.VarChar) { Value = StoreID },
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


        [HttpGet("getProductInfoByProductID")]
        public async Task<IActionResult> GetProductByProductID(string ProductID)
        {
            var product = await _context.Products.FindAsync(ProductID);

            if (product == null)
            {
                return NoContent();
            }
            return Ok(product);

        }


        [HttpGet("getProductStoresInfoByProductID")]
        public async Task<IActionResult> GetProductStoresInfoByProductID(string ProductID, int StoreID)
        {
            var product = await _context.Products_Stores.FirstOrDefaultAsync(p => ProductID == p.ProductID && StoreID == p.Store_ID);

            if (product == null)
            {
                return NoContent();
            }
            return Ok(product);

        }


        [HttpGet("getProductInfoByProductName")]
        public async Task<IActionResult> GetWarehouseProductByProductName(string productName)
        {
            var products = await _context.Products
                .Where(items => items.ProductName.Contains(productName))
                .ToListAsync();

            if (products == null)
            {
                return NoContent();
            }

            return Ok(products);
        }








    }
}
