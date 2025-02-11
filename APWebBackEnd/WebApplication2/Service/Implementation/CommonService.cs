using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using APWeb.Service.Interface;
using APWeb.Service.Models;
using System;
using System.Threading.Tasks;
using APWeb.Models;

namespace APWeb.Service.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly ApplicationDbContext _context;

        public CommonService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves the NodeID (ServerID) of the database server.
        /// </summary>
        /// <returns>The NodeID of the database server as an integer.</returns>
        public async Task<ServiceResult<int>> GetServerNodeIDAsync()
        {
            try
            {
                // Step 1: Retrieve the database server name using ExecuteScalarAsync
                string serverName;
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT @@SERVERNAME";
                    command.CommandType = System.Data.CommandType.Text;

                    await _context.Database.OpenConnectionAsync();
                    serverName = (string)await command.ExecuteScalarAsync();
                }

                if (string.IsNullOrEmpty(serverName))
                {
                    return ServiceResult<int>.FailureResult("Failed to retrieve the database server name.");
                }

                // Step 2: Query the Nodes table to fetch the NodeID for the database server
                var serverNode = await _context.Nodes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.NodeName == serverName);

                if (serverNode == null)
                {
                    return ServiceResult<int>.FailureResult($"No NodeID found for the database server '{serverName}'.");
                }

                // Step 3: Return the NodeID
                return ServiceResult<int>.SuccessResult(serverNode.NodeID, "Get Server NodeID successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.FailureResult($"Error retrieving Server NodeID: {ex.Message}");
            }
            finally
            {
                // Ensure the connection is closed
                await _context.Database.CloseConnectionAsync();
            }
        }
    }
}
