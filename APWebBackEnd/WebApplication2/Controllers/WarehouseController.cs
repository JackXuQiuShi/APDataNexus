
using APWeb.Dtos;
using APWeb.Mappers;
using APWeb.Models;
using APWeb.Repos;
using APWeb.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController(IConfiguration configuration, ApplicationDbContext context, HMRRepository HMRrepo, WarehouseRepository whrepo, IGeneralService service) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly string sqlDataSource = configuration.GetConnectionString("pris23Con");
        private readonly ApplicationDbContext _context = context;
        private readonly HMRRepository _repo = HMRrepo;
        private readonly WarehouseRepository _whrepo = whrepo;
        private readonly IGeneralService _service = service;

        /*        [HttpGet("getWarehouseByInvoice")]
                public async Task<JsonResult> GetWarehouseByInvoice(string Invoice, int StatusID)
                {
                    string query = "SELECT * FROM dbo.WarehouseInvoice WHERE Invoice = @Invoice AND StatusID = @StatusID;";
                    SqlParameter[] parameters = {
                        new SqlParameter("@Invoice", SqlDbType.VarChar) { Value = Invoice },
                        new SqlParameter("@StatusID", SqlDbType.Int) { Value = StatusID },
                    };
                    DataTable result = await ExecuteQueryAsync(query, parameters);
                    return new JsonResult(result);
                }*/

        // [HttpGet("getWarehouseStorage")]
        // public async Task<JsonResult> getWarehouseStorage()
        // {
        //     string query = "SELECT * FROM dbo.WarehouseStorage " +
        //         "ORDER BY Date DESC";

        //     DataTable result = await ExecuteQueryAsync(query, null);
        //     return new JsonResult(result);
        // }

        // [HttpGet("getCaseQty")]
        // public async Task<JsonResult> GetCaseQty(string ProductID)
        // {
        //     string query = "SELECT CaseQty FROM dbo.WarehouseStorage WHERE ProductID = @ProductID;";
        //     SqlParameter[] parameters = {
        //         new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID }
        //     };
        //     DataTable result = await ExecuteQueryAsync(query, parameters);
        //     return new JsonResult(result);
        // }

        // /* [HttpGet("getHQInfo")]
        //  public async Task<JsonResult> GetHQInfo(string ProductID)
        //  {
        //      string query = "SELECT P.Prod_Name, PP.RegPrice, PD.ReceivingDate AS Latest_ReceivingDate, PD.PriceReceived,PD.UnitsPerPackage " +
        //          "FROM dbo.Products P " +
        //          "LEFT JOIN dbo.ProductPrice PP ON P.Prod_Num = PP.ProdNum " +
        //          "LEFT JOIN (SELECT Product_ID, ReceivingDate, PriceReceived, UnitsPerPackage, ROW_NUMBER() OVER (PARTITION BY Product_ID ORDER BY ReceivingDate DESC) AS RowNum FROM dbo.PO_Details) PD ON P.Prod_Num = PD.Product_ID AND PD.RowNum = 1" +
        //          "WHERE P.Prod_Num = @ProductID;";

        //      SqlParameter[] parameters = {
        //          new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = ProductID }
        //      };

        //      DataTable result = await ExecuteQueryAsync(query, parameters);
        //      return new JsonResult(result);
        //  }*/


        // /* [HttpGet("getHQProductSupplier")]
        //  public async Task<JsonResult> getHQProductSupplier(string ProductID)
        //  {
        //      string query = "SELECT TOP 1 Suppliers.CompanyName, POs.Supplier_ID FROM POs " +
        //          "LEFT JOIN Suppliers ON POs.Supplier_ID = Suppliers.Supplier_ID " +
        //          "LEFT JOIN PO_Details ON POs.PO_ID = PO_Details.PO_ID " +
        //          "WHERE PO_Details.Product_ID = @ProductID " +
        //          "ORDER BY POs.PODraftDate DESC;";

        //      SqlParameter[] parameters = {
        //          new SqlParameter("@ProductID", ProductID)
        //      };

        //      DataTable result = await ExecuteQueryAsync(query, parameters);
        //      return new JsonResult(result);
        //  }*/

        // private async Task<IActionResult> InsertWarehouseItem(Warehouse warehouse)
        // {
        //     string query = "INSERT INTO dbo.WarehouseStorage(ProductID,UnitCost,CaseQty,ProductName,SupplierID,Applicant,Date,Location,UnitsPerPackage,UnitQty) " +
        //                    "VALUES (@ProductID,@UnitCost,@CaseQty,@ProductName,@SupplierID,@Applicant,@Date,@Location,@UnitsPerPackage,@UnitQty)";

        //     SqlParameter[] parameters = {
        //         new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = warehouse.ProductID },
        //         new SqlParameter("@UnitCost", SqlDbType.Float) { Value = warehouse.UnitCost },
        //         new SqlParameter("@CaseQty", SqlDbType.Int) { Value = warehouse.CaseQty },
        //         new SqlParameter("@ProductName", SqlDbType.VarChar) { Value = warehouse.ProductName },
        //         new SqlParameter("@SupplierID", SqlDbType.Int) { Value = warehouse.SupplierID },
        //         new SqlParameter("@Applicant", SqlDbType.VarChar) { Value = warehouse.Applicant },
        //         new SqlParameter("@Date", SqlDbType.DateTime) { Value = DateTime.Now },
        //         new SqlParameter("@Location", SqlDbType.VarChar) { Value = warehouse.Location },
        //         new SqlParameter("@UnitsPerPackage", SqlDbType.Int) { Value = warehouse.UnitsPerPackage },
        //         new SqlParameter("@UnitQty", SqlDbType.Int) { Value = warehouse.UnitsPerPackage * warehouse.CaseQty }
        //     };

        //     int affectedRows = await ExecuteNonQueryAsync(query, parameters);
        //     if (affectedRows > 0)
        //     {
        //         return Ok(new { message = "Insert successful." });
        //     }
        //     return BadRequest(new { message = "Insert failed." });
        // }

        // private async Task<IActionResult> UpdateWarehouseItem(Warehouse warehouse)
        // {
        //     string query = "UPDATE dbo.WarehouseStorage SET CaseQty = @CaseQty, UnitCost = @UnitCost, ProductName = @ProductName, SupplierID = @SupplierID, Applicant = @Applicant, Location = @Location, UnitsPerPackage = @UnitsPerPackage, UnitQty = @UnitQty " +
        //        "WHERE ProductID = @ProductID;";

        //     SqlParameter[] parameters = {
        //             new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = warehouse.ProductID },
        //             new SqlParameter("@CaseQty", SqlDbType.Int) { Value = warehouse.CaseQty },
        //             new SqlParameter("@UnitCost", SqlDbType.Float) { Value = warehouse.UnitCost },
        //             new SqlParameter("@ProductName", SqlDbType.VarChar) { Value = warehouse.ProductName },
        //             new SqlParameter("@SupplierID", SqlDbType.Int) { Value = warehouse.SupplierID },
        //             new SqlParameter("@Applicant", SqlDbType.VarChar) { Value = warehouse.Applicant },
        //             new SqlParameter("@Location", SqlDbType.VarChar) { Value = warehouse.Location },
        //             new SqlParameter("@UnitsPerPackage", SqlDbType.Int) { Value = warehouse.UnitsPerPackage },
        //             new SqlParameter("@UnitQty", SqlDbType.Int) { Value = warehouse.UnitsPerPackage * warehouse.CaseQty }
        //         };

        //     int affectedRows = await ExecuteNonQueryAsync(query, parameters);
        //     if (affectedRows > 0)
        //     {
        //         return Ok(new { message = "Update successful." });
        //     }
        //     return BadRequest(new { message = "Update failed." });
        // }

        // [HttpPost("submitWarehouseItem")]
        // public async Task<IActionResult> SubmitWarehouseItem(Warehouse warehouse)
        // {
        //     bool exists = await IsExist(warehouse);
        //     if (exists)
        //     {
        //         return await UpdateWarehouseItem(warehouse);
        //     }
        //     else
        //     {
        //         return await InsertWarehouseItem(warehouse);
        //     }
        // }

        // /*[HttpPost("insertWarehouseInvoice")]
        // public async Task<IActionResult> InsertWarehouseInvoice(Warehouse warehouse)
        // {
        //     string query = "INSERT INTO dbo.WarehouseInvoice(Invoice,SupplierID,ProductID,BuyerID,UnitCost,UnitsPerPackage,UnitQty,Applicant,InvoiceDate,StatusID,ProductName) " +
        //                    "VALUES (@Invoice,@SupplierID,@ProductID,@BuyerID,@UnitCost,@UnitsPerPackage,@UnitQty,@Applicant,@InvoiceDate,0,@ProductName)";

        //     SqlParameter[] parameters = {
        //          new SqlParameter("@Invoice", SqlDbType.VarChar) { Value = warehouse.Invoice },
        //          new SqlParameter("@SupplierID", SqlDbType.Int) { Value = warehouse.SupplierID },
        //          new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = warehouse.ProductID },
        //          new SqlParameter("@BuyerID", SqlDbType.Int) { Value = warehouse.BuyerID },
        //          new SqlParameter("@UnitCost", SqlDbType.Float) { Value = warehouse.UnitCost },
        //          new SqlParameter("@UnitsPerPackage", SqlDbType.Int) { Value = warehouse.UnitsPerPackage },
        //          new SqlParameter("@UnitQty", SqlDbType.Int) { Value = warehouse.UnitQty },
        //          new SqlParameter("@Applicant", SqlDbType.VarChar) { Value = warehouse.Applicant },
        //          new SqlParameter("@InvoiceDate", SqlDbType.Date) { Value = DateTime.Now },
        //          new SqlParameter("@ReceivingDate", SqlDbType.DateTime) { Value = DateTime.Now },
        //          new SqlParameter("@ProductName", SqlDbType.VarChar) { Value = warehouse.ProductName }
        //      };

        //     int affectedRows = await ExecuteNonQueryAsync(query, parameters);
        //     if (affectedRows > 0)
        //     {
        //         return Ok(new { message = "Insert successful." });
        //     }
        //     return BadRequest(new { message = "Insert failed." });
        // }*/


        // /* [HttpPost("comfirmInvoice")]
        //  public async Task<IActionResult> ComfirmInvoice(string Invoice)
        //  {
        //      string query = "UPDATE dbo.WarehouseInvoice SET ReceivedDate = @ReceivedDate, StatusID = 1 " +
        //         "WHERE Invoice = @Invoice;";

        //      SqlParameter[] parameters = {
        //                     new SqlParameter("@ReceivedDate", SqlDbType.VarChar) { Value = DateTime.Now },
        //                     new SqlParameter("@Invoice", SqlDbType.VarChar) { Value = Invoice }
        //             };

        //      int affectedRows = await ExecuteNonQueryAsync(query, parameters);
        //      if (affectedRows > 0)
        //      {
        //          return Ok(new { message = "Update successful." });
        //      }
        //      return BadRequest(new { message = "Update failed." });
        //  }*/



        // [HttpPost("isExist")]
        // public async Task<bool> IsExist(Warehouse warehouse)
        // {
        //     string query = "SELECT COUNT(1) FROM dbo.WarehouseStorage " +
        //         "WHERE ProductID = @ProductID;";

        //     SqlParameter[] parameters = {
        //         new SqlParameter("@ProductID", SqlDbType.VarChar) { Value = warehouse.ProductID }
        //     };

        //     object result = await ExecuteScalarAsync(query, parameters);
        //     return (int)result > 0;
        // }





        // private async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[] parameters)
        // {
        //     DataTable table = new();
        //     using (SqlConnection myCon = new(sqlDataSource))
        //     {
        //         await myCon.OpenAsync();
        //         using (SqlCommand myCommand = new(query, myCon))
        //         {
        //             if (parameters != null)
        //             {
        //                 myCommand.Parameters.AddRange(parameters);
        //             }
        //             using SqlDataReader myReader = await myCommand.ExecuteReaderAsync();
        //             table.Load(myReader);
        //         }
        //     }
        //     return table;
        // }


        // private async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] parameters)
        // {
        //     using SqlConnection myCon = new(sqlDataSource);
        //     await myCon.OpenAsync();
        //     using SqlCommand myCommand = new(query, myCon)
        //     {
        //         CommandType = CommandType.Text
        //     };
        //     if (parameters != null)
        //     {
        //         myCommand.Parameters.AddRange(parameters);
        //     }
        //     return await myCommand.ExecuteNonQueryAsync();
        // }


        // private async Task<object> ExecuteScalarAsync(string query, SqlParameter[] parameters)
        // {
        //     using (SqlConnection myCon = new(sqlDataSource))
        //     {
        //         await myCon.OpenAsync();
        //         using (SqlCommand myCommand = new(query, myCon))
        //         {
        //             if (parameters != null)
        //             {
        //                 myCommand.Parameters.AddRange(parameters);
        //             }
        //             return await myCommand.ExecuteScalarAsync();
        //         }
        //     }
        // }


        // [HttpGet("getHMRProducts")]
        // public async Task<IActionResult> GetHMRProducts()
        // {
        //     var products = await _context.HMRProducts.Include(p => p.Supplier).ToListAsync();
        //     return Ok(products.Select(p => p.HMRProductToDto()));
        // }

        // [HttpPost("addInHMR")]
        // public async Task<IActionResult> AddInHMR([FromBody] AddInHMRDto record)
        // {
        //     // TODO: Add record validation
        //     var product = _context.HMRProducts.FirstOrDefault(p => p.ProductID == record.ProductID);

        //     if (product == null)
        //     {
        //         return NotFound($"ProductID not found: {record.ProductID}");
        //     }

        //     // check if warehouseInventory exist
        //     var wi_today = await _context.HMRInventory.FirstOrDefaultAsync(item => item.ProductID == record.ProductID && item.Date == record.Date);

        //     if (wi_today == null)
        //     {
        //         await _context.AddAsync(record.AddInDtoToHMRInventory(product));
        //     }
        //     else
        //     {
        //         wi_today.UnitQty = wi_today.UnitQty + record.UnitQty;
        //     }

        //     await _context.AddAsync(record.AddInDtoToHMRTransactions(product));
        //     await _context.SaveChangesAsync();

        //     /*return Ok(record.addInDtoToInventoryTransactions(product.ProductName));*/
        //     return Ok(new { message = "Insert successful." });
        // }


        // [HttpPost("takeOutHMR")]
        // public async Task<IActionResult> TakeOutHMR([FromBody] TakeOutHMRDto record)
        // {
        //     // TODO: Add record validation
        //     // check数量是否足够
        //     var totalQty = await _repo.GetProductQty(record.ProductID);
        //     if (totalQty < record.UnitQty)
        //     {
        //         return BadRequest("Take out too much.");
        //     }

        //     // check if the product exist
        //     var product = await _repo.FindProduct(record.ProductID);
        //     if (product == null)
        //     {
        //         return BadRequest("Product Not Exist");
        //     }

        //     var qty = record.UnitQty;

        //     // pagination variables
        //     var position = 0;
        //     var num = 5; // 每页有多少个record

        //     while (qty > 0)
        //     {
        //         // 每次获取五个
        //         var inventories = await _context.HMRInventory
        //             .Where(wi => wi.ProductID == record.ProductID && wi.UnitQty != 0)
        //             .OrderBy(wi => wi.Date)
        //             .Skip(position)
        //             .Take(num)
        //             .ToListAsync();


        //         for (var i = 0; i < inventories.Count; i++)
        //         {
        //             if (inventories[i].UnitQty >= qty)
        //             {
        //                 // If the current inventory record has enough quantity
        //                 // 如果这条inventory足够了
        //                 inventories[i].UnitQty = inventories[i].UnitQty - qty; // update inventory还剩多少
        //                 await _context.AddAsync(record.TakeOutDtoToHMRTransactions(product, qty, inventories[i].Date)); // log the transaction

        //                 qty = 0; // update还需要take out多少qty。应为0
        //                 break;
        //             }
        //             else
        //             {
        //                 // If the current inventory record does not have enough quantity
        //                 // 如果当前这条的inventory的qty不够
        //                 qty = qty - inventories[i].UnitQty; // update还需要take out多少qty
        //                 await _context.AddAsync(record.TakeOutDtoToHMRTransactions(product, inventories[i].UnitQty, inventories[i].Date)); // log the transaction
        //                 inventories[i].UnitQty = 0; // update inventory还剩多少。应该为0
        //             }

        //         }
        //         position += num; // 如果还不够，move to next page
        //     }

        //     await _context.SaveChangesAsync();

        //     return Ok(new { message = "Inventory updated successfully." });

        // }


        // [HttpGet("getHMRProductByProductID")]
        // public async Task<IActionResult> GetHMRProductByProductID(string productID)
        // {
        //     var product = await _context.HMRProducts
        //         .Include(x => x.Supplier)
        //         .FirstOrDefaultAsync(items => items.ProductID == productID);

        //     if (product == null)
        //     {
        //         return NoContent();
        //     }


        //     return Ok(product.HMRProductToDto());
        // }


        // [HttpGet("getHMRProductByProductName")]
        // public async Task<IActionResult> GetHMRProductByProductName(string productName)
        // {
        //     var products = await _context.HMRProducts
        //         .Include(p => p.Supplier)
        //         .Where(items => items.ProductName.Contains(productName))
        //         .ToListAsync();

        //     if (products == null)
        //     {
        //         return NoContent();
        //     }

        //     return Ok(products.Select(p => p.HMRProductToDto()));
        // }


        // [HttpGet("getHMRInventoryByProductID")]
        // public async Task<IActionResult> GetHMRInventoryProductID(string productID)
        // {
        //     var inventoryItems = await _context.HMRInventory
        //                                .Where(item => item.ProductID == productID && item.UnitQty > 0)
        //                                .OrderByDescending(item => item.Date)
        //                                .ToListAsync();

        //     return Ok(inventoryItems);
        // }


        // [HttpGet("getHMRTransactionByProductID")]
        // public async Task<IActionResult> GetHMRTransactionByProductID(string productID)
        // {
        //     var today = DateTime.Today;

        //     var transactionItems = await _context.HMRTransactions
        //         .Where(item => item.ProductID == productID && item.UnitQty > 0 && item.Date >= today && item.Date < today.AddDays(1)) // 获取当天所有这个产品的Transaction
        //         .OrderByDescending(item => item.Date)
        //         .ToListAsync();

        //     return Ok(transactionItems);
        // }

        // [HttpGet("getHMRTotalQtyByProductID")]
        // public async Task<IActionResult> GetHMRTotalQtyByProductID(string productID)
        // {
        //     var totalQty = await _repo.GetProductQty(productID);

        //     return Ok(totalQty);
        // }

        // [HttpGet("getWarehouseDrafts")]
        // public async Task<IActionResult> GetWarehouseDrafts(int StoreID)
        // {
        //     var draft = await _context.WarehouseDraft
        //         .Where(draft => draft.StoreID == StoreID && draft.StatusID == 0)
        //         .ToListAsync();

        //     if (draft == null)
        //     {
        //         return NoContent();
        //     }

        //     return Ok(draft);
        // }


        // [HttpPost("createWarehouseDraft")]
        // public async Task<IActionResult> CreateWarehouseDraft(CreateWarehouseDraftDto record)
        // {
        //     var product = await _context.ProductItems.FindAsync(record.UPC);
        //     if (product == null) { return NoContent(); }
        //     var draft = record.CreateWarehouseDraftDtoToDraft(product);
        //     await _context.AddAsync(draft);
        //     await _context.SaveChangesAsync();
        //     return Ok();
        // }


        // [HttpPost("addInWarehouse")]
        // public async Task<IActionResult> AddInWarehouse(AddInWarehouseDto adto)
        // {
        //     try
        //     {
        //         // Call the repository method to add a single warehouse inventory item
        //         var result = await _whrepo.AddInWarehouse(adto);
        //         return Ok(new { message = "Insert successful.", inventoryID = result });
        //     }
        //     catch (Exception ex)
        //     {
        //         // Return a bad request with the error message in case of an exception
        //         return BadRequest(new { message = ex.Message });
        //     }
        // }

        // [HttpPost("addInWarehouseBatch")]
        // public async Task<IActionResult> AddInWarehouseBatch(List<AddInWarehouseDto> addInWarehouseDtos)
        // {
        //     try
        //     {
        //         // Call the repository method to add multiple warehouse inventory items in a batch
        //         var result = await _whrepo.AddInWarehouseBatch(addInWarehouseDtos);
        //         return Ok(new { message = "Batch insert successful.", inventoryIDs = result });
        //     }
        //     catch (Exception ex)
        //     {
        //         // Return a bad request with the error message in case of an exception
        //         return BadRequest(new { message = ex.Message });
        //     }
        // }

        // [HttpPost("takeOutWarehouse")]
        // public async Task<IActionResult> TakeOutWarehouse([FromBody] TakeOutWarehouseDto tdto)
        // {
        //     if (tdto == null || string.IsNullOrEmpty(tdto.ProductID))
        //     {
        //         // Return a bad request if the provided data is invalid
        //         return BadRequest("Invalid data provided.");
        //     }

        //     try
        //     {
        //         await _whrepo.TakeOutWarehouse(tdto);
        //         return Ok(new { message = "Product successfully taken out of the warehouse." });
        //     }
        //     catch (Exception ex)
        //     {
        //         // Return a bad request with the error message in case of an exception
        //         return BadRequest(new { message = ex.Message });
        //     }
        // }

        // [HttpPost("takeOutWarehouseBatch")]
        // public async Task<IActionResult> TakeOutWarehouseBatch([FromBody] List<TakeOutWarehouseDto> tdtoList)
        // {
        //     if (tdtoList == null || tdtoList.Count == 0)
        //     {
        //         // Return a bad request if the provided data is invalid
        //         return BadRequest("Invalid data provided.");
        //     }

        //     try
        //     {
        //         // Call the repository method to take out multiple warehouse inventory items in a batch
        //         var result = await _whrepo.TakeOutWarehouseBatch(tdtoList);
        //         return Ok(new { message = "Products successfully taken out of the warehouse." });
        //     }
        //     catch (Exception ex)
        //     {
        //         // Return a bad request with the error message in case of an exception
        //         return BadRequest(new { message = ex.Message });
        //     }
        // }

        // [HttpGet("getWarehouseProductQty")]
        // public async Task<IActionResult> GetWarehouseStoreProductQty(string ProductID, int storeID)
        // {
        //     return Ok(await _whrepo.GetStoreProductQty(ProductID, storeID));
        // }


        // [HttpGet("getWarehouseTransactionSummary")]
        // public async Task<IActionResult> GetWarehouseTransactionSummary(string? productID)
        // {

        //     var transactions_query = _context.WarehouseTransactions
        //         .GroupBy(t => new { t.ProductID, t.Date.Date, t.Action, t.StoreID, t.POID, t.SellTo });


        //     if (productID != null)
        //     {
        //         transactions_query = transactions_query.Where(g => g.Key.ProductID == productID);
        //     }

        //     var transactions = await transactions_query
        //         .Select(g => new
        //         {
        //             g.Key.Date.Date,
        //             g.Key.ProductID,
        //             g.Key.Action,
        //             g.Key.StoreID,
        //             g.Key.POID,
        //             g.Key.SellTo,
        //             UnitQty = g.Sum(t => t.UnitQty),
        //             ProductName = g.Where(t => t.ProductName != null).Select(t => t.ProductName).FirstOrDefault(),
        //         })
        //         .ToListAsync();
        //     return Ok(transactions);
        // }


        // [HttpGet("getAllWarehouseTransation")]
        // public async Task<IActionResult> GetAllWarehouseTransation()
        // {
        //     var transactions = await _context.WarehouseTransactions.ToListAsync();
        //     return Ok(transactions);
        // }

        // [HttpPost("undoWarehouseTransactionByTransactionID")]
        // public async Task<IActionResult> UndoWarehouseTransactionByTransactionID(int transactionID)
        // {
        //     try
        //     {
        //         // Call the UndoTransaction method
        //         int undoTransactionId = await _whrepo.UndoTransaction(transactionID);

        //         // Return a 200 OK response with the ID of the new undo transaction
        //         return Ok(new { UndoTransactionID = undoTransactionId });
        //     }
        //     catch (DbUpdateException dbEx)
        //     {
        //         // Log or return the detailed inner exception message
        //         var detailedError = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
        //         return BadRequest(new { Error = detailedError });
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(new { Error = ex.Message });
        //     }
        // }

        /*        [HttpGet("GetSupplier")]
                public async Task<IActionResult> GetSupplier(int supplierID)
                {
                    var supplier = await _context.Suppliers
                                               .Where(s => s.Supplier_ID == supplierID)
                                               .FirstOrDefaultAsync();

                            return Ok(supplier);
                        }*/

        // [HttpGet("getWarehouseInventoryByProductName")]
        // public async Task<IActionResult> GetWarehouseInventoryProductName(string productName)
        // {
        //     var inventoryItems = await _context.WarehouseInventory
        //                                 .Where(p => p.ProductName.Contains(productName))
        //                                 .OrderBy(p => EF.Functions.Like(p.ProductName, productName) ? 0 : 1)
        //                                 .FirstOrDefaultAsync();

        //     return Ok(inventoryItems);
        // }



        /* [HttpGet("getWarehouseTransactionByProductName")]
         public async Task<IActionResult> GetWarehouseTransactionByProductName(string productName)
         {
             var transactionItems = await _context.WarehouseTransactions
                                         .Where(p => p.ProductName.Contains(productName))
                                         .OrderBy(p => EF.Functions.Like(p.ProductName, productName) ? 0 : 1)
                                         .FirstOrDefaultAsync();

             return Ok(transactionItems);
         }*/


        // [HttpPost("undoWarehouseTransactionByDto")]
        // public async Task<IActionResult> UndoWarehouseTransactionByDto([FromBody] UndoWarehouseTransactionDto dto)
        // {
        //     try
        //     {
        //         // Instantiate the service where the UndoWarehouseTransactionByDto method is implemented

        //         // Call the UndoWarehouseTransactionByDto method
        //         var undoTransactionIds = await _whrepo.UndoWarehouseTransactionByDto(dto);

        //         // Return a 200 OK response with the list of undo transaction IDs
        //         return Ok(new { UndoTransactionIDs = undoTransactionIds });
        //     }
        //     catch (Exception ex)
        //     {
        //         // Return a 400 Bad Request response with the error message
        //         return BadRequest(new { Error = ex.Message });
        //     }
        // }


        // [HttpPost("adjustWarehouseInventory")]
        // public async Task<IActionResult> AdjustWarehouseInventory([FromBody] AdjustWarehouseInventoryDto dto)
        // {
        //     try
        //     {
        //         // Call the AdjustWarehouseInventory method
        //         var result = await _whrepo.AdjustWarehouseInventory(dto);

        //         return Ok(new { AdjustmentID = result });
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(new { Error = ex.Message });
        //     }

        // var result = await _whrepo.AdjustWarehouseInventory(dto);

        // return Ok(new { AdjustmentID = result });
        // }





        // [HttpGet("getWarehouseInventory")]
        // public async Task<IActionResult> GetWarehouseInventory()
        // {
        //     var inventory = await _context.WarehouseInventory
        //     .GroupBy(i => new
        //     {
        //         i.ProductID,
        //         i.StoreID
        //     })
        //     .OrderByDescending(g => g.Max(i => i.Date))
        //     .Select(g => new
        //     {
        //         ProductID = g.Key.ProductID,
        //         StoreID = g.Key.StoreID,
        //         UnitQty = g.Sum(i => i.UnitQty),
        //         ProductName = g.Where(i => i.ProductName != null).Select(i => i.ProductName).FirstOrDefault()
        //     })
        //     .ToListAsync();
        //     return Ok(inventory);
        // }


        [HttpPost("ReceiveOrder")]
        public async Task<IActionResult> ReceiveOrder([FromBody] ReceiveOrderDto receiveOrderDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Step 1: Update Orders and OrderItems
                    var order = await _context.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefaultAsync(o => o.OrderID == receiveOrderDto.OrderID);

                    if (order == null)
                        return NotFound("Order not found");

                    // Update order status to 'Received'
                    order.OrderStatusID = 3;
                    order.DraftDate = DateTime.Now;

                    // Update each OrderItem with the received quantity and cost
                    foreach (var itemDto in receiveOrderDto.OrderItem)
                    {
                        var orderItem = order.OrderItems.FirstOrDefault(oi => oi.ProductItemID == itemDto.ProductItemID);

                        if (orderItem != null)
                        {
                            orderItem.UnitQty = itemDto.ReceivedQuantity;
                            orderItem.UnitCost = itemDto.ReceivedCost;
                        }
                    }

                    // Save changes to Orders and OrderItems
                    await _context.SaveChangesAsync();

                    // Step 2: Insert into ProductMovement
                    var productMovement = new ProductMovement
                    {
                        OrderID = receiveOrderDto.OrderID,
                        DraftDate = DateTime.Now,
                        MovementType = receiveOrderDto.MovementType, // For example, "Receiving"
                        Quantity = receiveOrderDto.OrderItem.Sum(item => item.ReceivedQuantity),
                        SourceStorageAreaID = order.SourceStorageAreaID,
                        DestinationStorageAreaID = order.DestinationStorageAreaID
                    };

                    _context.ProductMovements.Add(productMovement);
                    await _context.SaveChangesAsync(); // Save to generate MovementID

                    // Step 3: Insert into ProductMovementItems
                    foreach (var itemDto in receiveOrderDto.OrderItem)
                    {
                        var productMoveItem = new ProductMovementItem
                        {
                            MovementID = productMovement.MovementID,
                            ProductItemID = itemDto.ProductItemID,
                            SourceWarehouseLocationID = itemDto.SourceWarehouseLocationID,
                            DestinationWarehouseLocationID = itemDto.DestinationWarehouseLocationID,
                            UnitQty = itemDto.ReceivedQuantity,
                            UnitCost = itemDto.ReceivedCost
                        };

                        _context.ProductMovementItems.Add(productMoveItem);
                    }

                    await _context.SaveChangesAsync();

                    // Step 4: Update Inventory
                    foreach (var itemDto in receiveOrderDto.OrderItem)
                    {
                        var inventoryItem = await _context.Inventories
                            .FirstOrDefaultAsync(i => i.ItemID == itemDto.ItemID && i.LocationID == itemDto.DestinationWarehouseLocationID);

                        if (inventoryItem != null)
                        {
                            // Update existing inventory
                            inventoryItem.CurrentStock += itemDto.ReceivedQuantity;
                            inventoryItem.CurrentCost = itemDto.ReceivedCost;
                        }
                        else
                        {
                            // Add new inventory record
                            _context.Inventories.Add(new Inventory
                            {
                                ItemID = itemDto.ItemID,
                                LocationID = itemDto.DestinationWarehouseLocationID,
                                CurrentStock = itemDto.ReceivedQuantity,
                                CurrentCost = itemDto.ReceivedCost
                            });
                        }
                    }

                    await _context.SaveChangesAsync();

                    // Step 5: Commit transaction
                    await transaction.CommitAsync();
                    return Ok("Order received and inventory updated successfully.");
                }
                catch (Exception ex)
                {
                    // Step 6: Rollback transaction in case of failure
                    await transaction.RollbackAsync();
                    return BadRequest(new { message = ex.Message });
                }
            }
        }
        



    }
}
