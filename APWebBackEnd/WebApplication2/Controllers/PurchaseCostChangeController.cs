
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
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using APWeb.TempModels;


namespace APWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseCostChangeController(IConfiguration configuration, IGeneralService generalService, ApplicationDbContext context) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IGeneralService _generalService = generalService;

        /*private readonly string sqlDataSource = configuration.GetConnectionString("PRIS_ALP_test");*/
        private readonly string sqlDataSource = configuration.GetConnectionString("pris23Con");
        private readonly ApplicationDbContext _context = context;

        [HttpPost("draftNewRequest")]
        public async Task<IActionResult> DraftNewRequest(string productId, string costType, decimal newCost, int draftUserId)
        {
            try {
                decimal order = await _context.Prices
                    .Where(p => p.Prodnum.Contains(productId))
                    .Select(p => p.Order)
                    .FirstOrDefaultAsync() ?? 0;
                var newRequest = new PurchaseCostChangeRequest
                {
                    ProductId = productId,
                    ChangeId = (_context.PurchaseCostChangeRequests.Any()
                        ? _context.PurchaseCostChangeRequests.Max(p => p.ChangeId) + 1
                        : 1),
                    CostType = 1,
                    StatusId = 1,
                    DraftDate = DateTime.Now,
                    SubmitDate = DateTime.Now,
                    CompleteDate = DateTime.Now,
                    DraftUserId = draftUserId,
                    SubmitUserId = 0,
                    CompleteUserId = 0,
                    UnitCostOld = order,
                    UnitCostNew = newCost
                };
                await _context.PurchaseCostChangeRequests.AddAsync(newRequest);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Update successful." });
            } catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while drafting.", error = ex.Message });
            }
        }



        [HttpPost("submitCostChange")]
        public async Task<IActionResult> SubmitCostChange(string productId, int changeId, int userId)
        {
            try
            {
                var request = await _context.PurchaseCostChangeRequests
                    .FirstOrDefaultAsync(r => r.ProductId == productId && r.ChangeId == changeId);
                var history = await _context.PurchaseCostChangeRequests
                .Where(r => r.ProductId == productId)
                .ToListAsync();
                bool hasSubmittedRecord = false;

                foreach (var record in history)
                {
                    if (record.StatusId == 2)
                    {
                        hasSubmittedRecord = true;
                        break; // Exit loop early since at least one record is found
                    }
                }

                if (hasSubmittedRecord)
                {
                    Console.WriteLine("Submission denied: A record with StatusId = 2 already exists.");
                    return BadRequest(new { message = "Update unsuccessful." }); 
                }


                if (request != null)
                {
                    request.StatusId = 2;
                    request.SubmitDate = DateTime.Now;
                    request.SubmitUserId = userId;

                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Update successful." });
                }
                return BadRequest(new { message = "Update unsuccessful." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the products.", error = ex.Message });
            }
        }

        [HttpPost("approveCostChange")]
        public async Task<IActionResult> ApproveCostChange(string productId, int changeId, int userId, decimal newPrice)
        {
            try
            {
                var request = await _context.PurchaseCostChangeRequests
                    .FirstOrDefaultAsync(r => r.ProductId == productId && r.ChangeId == changeId);
                var priceRequest = await _context.Prices
                    .FirstOrDefaultAsync(r => r.Prodnum.Contains(productId));

                if (request != null && priceRequest != null)
                {
                    request.StatusId = 3;
                    request.CompleteDate = DateTime.Now;
                    request.CompleteUserId = userId;

                    _context.Prices.Attach(priceRequest);
                    priceRequest.Order = newPrice;

                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Update successful." });
                }
                return BadRequest(new { message = "Update unsuccessful." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the products.", error = ex.Message });
            }
        }

        [HttpPost("rejectCostChange")]
        public async Task<IActionResult> RejectCostChange(string productId, int changeId, int userId)
        {
            try
            {
                var request = await _context.PurchaseCostChangeRequests
                    .FirstOrDefaultAsync(r => r.ProductId == productId && r.ChangeId == changeId);

                if (request != null)
                {
                    request.StatusId = -1;
                    request.CompleteDate = DateTime.Now;
                    request.CompleteUserId = userId;

                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Update successful." });
                }
                return BadRequest(new { message = "Update unsuccessful." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the products.", error = ex.Message });
            }
        }

        [HttpPost("deletCostChange")]
        public async Task<IActionResult> DeletCostChange(string productId, int changeId)
        {
            try
            {
                var request = await _context.PurchaseCostChangeRequests
                    .FirstOrDefaultAsync(r => r.ProductId == productId && r.ChangeId == changeId);

                if (request != null)
                {
                    _context.PurchaseCostChangeRequests.Remove(request);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Update successful." });
                }
                return BadRequest(new { message = "Update unsuccessful." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the products.", error = ex.Message });
            }
        }

        [HttpPost("editCostChange")]
        public async Task<IActionResult> EditCostChange(string productId, int changeId, decimal newCost)
        {
            try
            {
                var request = await _context.PurchaseCostChangeRequests
                    .FirstOrDefaultAsync(r => r.ProductId == productId && r.ChangeId == changeId);

                if(request != null)
                {
                    request.UnitCostNew = newCost;

                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Update successful." });
                }
                return BadRequest(new { message = "Update unsuccessful." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the products.", error = ex.Message });
            }
        }

        [HttpGet("getAllRecords")]
        public async Task<JsonResult> GetAllRecords(string productId)
        {
            var result = await _context.PurchaseCostChangeRequests
                    .ToListAsync();
            return new JsonResult(result);
        }

        [HttpGet("getPrice")]
        public async Task<JsonResult> GetPrice(string productId)
        {
            decimal result = await _context.Prices
                    .Where(p => p.Prodnum.Contains(productId))
                    .Select(p => p.Order)
                    .FirstOrDefaultAsync() ?? 0;

            return new JsonResult(result);
        }

        [HttpGet("getDraft")]
        public async Task<JsonResult> GetDraft(int draftUserId)
        {
            var requests = await _context.PurchaseCostChangeRequests
                .Where(r => r.StatusId == 1 && r.DraftUserId == draftUserId)
                .ToListAsync();

            return new JsonResult(requests);
        }

        [HttpGet("getPriceHistory")]
        public async Task<JsonResult> GetPriceHistory(string productId)
        {
            var requests = await _context.PurchaseCostChangeRequests
                .Where(r => productId.Contains(r.ProductId) && r.StatusId >= 2)
                .ToListAsync();

            return new JsonResult(requests);
        }

        [HttpGet("getSubmitted")]
        public async Task<JsonResult> GetSubmitted(int submitUserId)
        {
            var requests = await _context.PurchaseCostChangeRequests
                .Where(r => r.StatusId == 2 && r.SubmitUserId == submitUserId)
                .ToListAsync();

            return new JsonResult(requests);
        }

        [HttpGet("getApproved")]
        public async Task<JsonResult> GetApproved(int completeUserId)
        {
            var requests = await _context.PurchaseCostChangeRequests
                .Where(r => r.StatusId == 3 && r.CompleteUserId == completeUserId)
                .ToListAsync();

            return new JsonResult(requests);
        }

        [HttpGet("getRejected")]
        public async Task<JsonResult> GetRejected(int completeUserId)
        {
            var requests = await _context.PurchaseCostChangeRequests
                .Where(r => r.StatusId == -1 && r.CompleteUserId == completeUserId)
                .ToListAsync();

            return new JsonResult(requests);
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



    }
}
