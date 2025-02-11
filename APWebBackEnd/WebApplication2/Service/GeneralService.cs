using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace APWeb.Service
{
    public class GeneralService(IConfiguration configuration) : IGeneralService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly string sqlDataSource = configuration.GetConnectionString("pris23Con");

        public async Task<string> GetNormalizedID(string ProductID)
        {
                var normalizedID = await CallStoredProcedureAsync("Normalize_Barcode", ProductID);
                return normalizedID;
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
        }
















    }

}
