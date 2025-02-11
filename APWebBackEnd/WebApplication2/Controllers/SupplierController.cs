using APWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace APWeb.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet("getSupplierByName")]
        public JsonResult GetSupplierByName(string CompanyName)
        {
            string sqlDataSource = _configuration.GetConnectionString("pris23Con");
            //string sqlDataSource = _configuration.GetConnectionString("inventoryCon");
            DataTable table = new();
            SqlDataReader myReader;
            string query;

            /*query = "SELECT Supplier_ID, CompanyName, SafetyLicense, Phone, Fax, Email, Address, City, PostalCode  FROM dbo.Suppliers " +
                    "WHERE CompanyName LIKE '%' + @CompanyName + '%'";*/
            query = "SELECT Supplier_ID AS SupplierID, CompanyName, SafetyLicense, Phone, Fax, Email, Address, City, PostalCode FROM dbo.Suppliers " +
                    "WHERE CompanyName LIKE '%' + @CompanyName + '%'";

            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new(query, myCon))
                {
                    CompanyName ??= "";
                    myCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = CompanyName;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet("getSupplierRequest")]
        public JsonResult GetSupplierRequest(string CompanyName)
        {
            string sqlDataSource = _configuration.GetConnectionString("pris23Con");
            DataTable table = new();
            SqlDataReader myReader;
            string query;

            query = "SELECT * FROM dbo.Supplier_Request " +
                    "WHERE CompanyName LIKE '%' + @CompanyName + '%' AND Status = 'Pending'";

            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new(query, myCon))
                {
                    CompanyName ??= "";
                    myCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = CompanyName;

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet("getDeniedSupplier")]
        public JsonResult GetDeniedSupplier(string CompanyName)
        {
            string sqlDataSource = _configuration.GetConnectionString("pris23Con");
            DataTable table = new();
            SqlDataReader myReader;
            string query;

            query = "SELECT CompanyName, SafetyLicense, Phone, Fax, Email, Address, City, PostalCode  FROM dbo.Suppliers " +
                    "WHERE CompanyName LIKE '%' + @CompanyName + '%'";

            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new(query, myCon))
                {
                    CompanyName ??= "";
                    myCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = CompanyName;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost("insertSupplierRequest")]
        public IActionResult InsertSupplierRequest(Supplier supplier)
        {
            string sqlDataSource = _configuration.GetConnectionString("pris23Con");
            string query;

            query = "INSERT INTO dbo.Supplier_Request(CompanyName, Phone, Fax, Email, Address, City, PostalCode, SafetyLicense, Date, Status) " +
                    "VALUES (@CompanyName, @Phone, @Fax, @Email, @Address, @City, @PostalCode, @SafetyLicense, @Date, @Status)";

            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);

            myCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = supplier.CompanyName;
            myCommand.Parameters.Add("@Phone", SqlDbType.VarChar).Value = supplier.Phone;
            myCommand.Parameters.Add("@Fax", SqlDbType.VarChar).Value = supplier.Fax;
            myCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = supplier.Email;
            myCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = supplier.Address;
            myCommand.Parameters.Add("@City", SqlDbType.VarChar).Value = supplier.City;
            myCommand.Parameters.Add("@PostalCode", SqlDbType.VarChar).Value = supplier.PostalCode;
            myCommand.Parameters.Add("@SafetyLicense", SqlDbType.VarChar).Value = supplier.SafetyLicense;
            myCommand.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now;
            myCommand.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Pending";

            try
            {
                bool isExist = IsSupplierExist(supplier.CompanyName);
                if (isExist)
                {
                    return StatusCode(409, new { message = "Supplier already exists." });
                }
                int affectedRows = myCommand.ExecuteNonQuery();
                if (affectedRows > 0)
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

        [HttpPost("insertSupplier")]
        public IActionResult InsertSupplier(string CompanyName)
        {
            string sqlDataSource = _configuration.GetConnectionString("pris23Con");
            string query;

            query = "INSERT INTO dbo.Suppliers(Supplier_ID, CompanyName, Phone, Fax, Email, Address, City, PostalCode, SafetyLicense, Date) " +
                    "SELECT (SELECT MAX(Supplier_ID) FROM dbo.Suppliers) + 1 , CompanyName, Phone, Fax, Email, Address, City, PostalCode, SafetyLicense, Date FROM Supplier_Request WHERE CompanyName = @CompanyName";

            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);

            myCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = CompanyName;

            try
            {
                int affectedRows = myCommand.ExecuteNonQuery();
                if (affectedRows > 0)
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
                return StatusCode(500, new { message = e.Message });
            }
        }


        [HttpPost("updateSupplierRequest")]
        public IActionResult UpdateSupplierRequest(string CompanyName, int Status)
        {
            string sqlDataSource = _configuration.GetConnectionString("pris23Con");

            string query;

            query = "UPDATE dbo.Supplier_Request SET " +
                    "Status = @Status " +
                    "WHERE CompanyName = @CompanyName;";


            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);

            var statusValue = Status == 0 ? "Denied" : "Approved";

            myCommand.Parameters.Add("@Status", SqlDbType.VarChar).Value = statusValue;
            myCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = CompanyName;

            var affectedRows = myCommand.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                if (Status == 1)
                {
                    InsertSupplier(CompanyName);
                }
                return Ok(new { message = "Update successful." });
            }
            else
            {
                return NotFound(new { message = "Supplier not found or no changes made." });
            }
        }


        [HttpGet("isSupplierExist")]
        public bool IsSupplierExist(string CompanyName)
        {
            string sqlDataSource = _configuration.GetConnectionString("pris23Con");
            bool isExist = false;

            string querySupplier = "SELECT COUNT(1) FROM dbo.Suppliers " + // Note the added space before WHERE
                    "WHERE CompanyName = @CompanyName";

            string querySupplierRequest = "SELECT COUNT(1) FROM dbo.Supplier_Request " + // Note the added space before WHERE
                    "WHERE CompanyName = @CompanyName";

            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();

                // Check in dbo.Supplier_Request
                using (SqlCommand myCommand = new(querySupplierRequest, myCon))
                {
                    CompanyName ??= "";

                    myCommand.Parameters.AddWithValue("@CompanyName", CompanyName);
                    // ExecuteScalar used for single value
                    isExist = (int)myCommand.ExecuteScalar() > 0;
                }

                // If not found in dbo.Supplier_Request, then check in dbo.Supplier
                if (!isExist)
                {
                    using SqlCommand myCommand = new(querySupplier, myCon);
                    myCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = CompanyName;
                    // ExecuteScalar used for single value
                    isExist = (int)myCommand.ExecuteScalar() > 0;
                }

                myCon.Close();
            }
            return isExist;
        }





    }
}
