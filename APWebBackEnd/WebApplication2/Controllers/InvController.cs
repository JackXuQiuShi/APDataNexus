
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
    public class InvController(IConfiguration configuration, IGeneralService generalService, ApplicationDbContext context) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IGeneralService _generalService = generalService;

        /*private readonly string sqlDataSource = configuration.GetConnectionString("PRIS_ALP_test");*/
        private readonly string sqlDataSource = configuration.GetConnectionString("pris23Con");
        private readonly ApplicationDbContext _context = context;


        [HttpGet("getCostByItemIDAndLocationID")]
        public async Task<JsonResult> GetCostByItemIDAndLocationID(string LocationID, string ItemID)
        {

            string query = "SELECT CurrentCost FROM dbo.Inventory " +
                "where LocationID = @LocationID " +
                "and ItemID = @ItemID;";

            SqlParameter[] parameters = [
                new SqlParameter("@LocationID", SqlDbType.VarChar) { Value = LocationID },
                new SqlParameter("@ItemID", SqlDbType.VarChar) { Value = ItemID }
            ];
            DataTable result = await ExecuteQueryAsync(query, parameters);
            return new JsonResult(result);
        }

        [HttpPost("updateCost")]
        public async Task<IActionResult> UpdateCost(string newCost, string LocationID, string ItemID)
        {

            string query = "UPDATE dbo.Inventory " +
                "SET CurrentCost = @newCost "+
                "where LocationID = @LocationID and ItemID = @ItemID;";

            SqlParameter[] parameters = [
                new SqlParameter("@newCost", SqlDbType.VarChar) { Value = newCost },
                new SqlParameter("@LocationID", SqlDbType.VarChar) { Value = LocationID },
                new SqlParameter("@ItemID", SqlDbType.VarChar) { Value = ItemID }
            ];
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
