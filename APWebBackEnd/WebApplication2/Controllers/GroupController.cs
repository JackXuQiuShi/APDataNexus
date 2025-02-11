using APWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace APWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

  
        [HttpPost("insertGroup")]
        public IActionResult InsertGroup(ProductGroup group)
        {
            string sqlDataSource = _configuration.GetConnectionString("aphqsvr3_pris23Con");
            string query;

            query = "INSERT INTO dbo.ProduceGroup(ProduceGroupID, ProduceGroupName, ProduceGroupChineseName) " +
                    "VALUES ((SELECT MAX(ProduceGroupID) FROM dbo.ProduceGroup) + 1, UPPER(@ProduceGroupName), @ProduceGroupChineseName)";

            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);
          
            myCommand.Parameters.Add("@ProduceGroupName", SqlDbType.NVarChar).Value = group.ProduceGroupName;
            myCommand.Parameters.Add("@ProduceGroupChineseName", SqlDbType.NVarChar).Value = group.ProduceGroupChineseName;
        
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

        [HttpPost("insertGroupDetails")]
        public IActionResult InsertGroupDetails(List<GroupDetails> groupDetails)
        {
            string sqlDataSource = _configuration.GetConnectionString("aphqsvr3_pris23Con");
            string query = "INSERT INTO dbo.ProduceGroupDetails(ProduceGroupID, OrganizationID, Item_Nbr) VALUES (@ProduceGroupID, @OrganizationID, @Item_Nbr)";

            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();

            int affectedRows = 0;

            foreach (var detail in groupDetails)
            {
                using SqlCommand myCommand = new(query, myCon);
                
                myCommand.Parameters.Add("@ProduceGroupID", SqlDbType.Int).Value = detail.ProduceGroupID;
                myCommand.Parameters.Add("@OrganizationID", SqlDbType.Int).Value = detail.OrganizationID;
                myCommand.Parameters.Add("@Item_Nbr", SqlDbType.Int).Value = detail.Item_Nbr;

                affectedRows += myCommand.ExecuteNonQuery();
            }

            if (affectedRows == groupDetails.Count)
            {
                return Ok(new { message = "All inserts successful." });
            }
            else
            {
                return BadRequest(new { message = "Some inserts failed." });
            }
        }


        [HttpGet("getProductGroup")]
        public JsonResult GetProductGroup(string name)
        {
            string sqlDataSource = _configuration.GetConnectionString("aphqsvr3_pris23Con");
            DataTable table = new();
            SqlDataReader myReader;
            string query;

            query = "SELECT * FROM dbo.ProduceGroup WHERE ProduceGroupName LIKE '%" + name + "%' OR " +
                "ProduceGroupChineseName LIKE N'%" + name + "%'";

            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new(query, myCon);
                name ??= "";
                myCommand.Parameters.Add("@produceGroupName", SqlDbType.NVarChar).Value = name;

                myReader = myCommand.ExecuteReader();
                table.Load(myReader); ;

                myReader.Close();
                myCon.Close();
            }
            return new JsonResult(table);
        }

        [HttpPost("updateGroup")]
        public IActionResult UpdateGroup(ProductGroup group)
        {
            string sqlDataSource = _configuration.GetConnectionString("aphqsvr3_pris23Con");

            string query = "UPDATE dbo.ProduceGroup SET " +
                   "ProduceGroupName = UPPER(@ProduceGroupName), " +
                   "ProduceGroupChineseName = @ProduceGroupChineseName " +
                   "WHERE ProduceGroupID = @ProduceGroupID";

            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);

            myCommand.Parameters.Add("@ProduceGroupID", SqlDbType.Int).Value = group.ProduceGroupID;
            myCommand.Parameters.Add("@ProduceGroupName", SqlDbType.VarChar).Value = group.ProduceGroupName;
            myCommand.Parameters.Add("@ProduceGroupChineseName", SqlDbType.NVarChar).Value = group.ProduceGroupChineseName;

            int affectedRows = myCommand.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                return Ok(new { message = "Update successful." });
            }
            else
            {
                return NotFound(new { message = "Group not found or no changes made." });
            }
        }


        [HttpGet("getItemByName")]
        public JsonResult GetItemByName(string item_name)
        {
            string sqlDataSource = _configuration.GetConnectionString("aphqsvr3_pris23Con");
            DataTable table = new();
            SqlDataReader myReader;

            string query = "SELECT item.Item_Nbr, item.upc, item.item_desc_1 FROM dbo.Item item " +
                "WHERE item.item_desc_1 LIKE @item_name AND item.department_id = 94";

            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new(query, myCon);
                myCommand.Parameters.Add("@item_name", SqlDbType.VarChar).Value = "%" + item_name + "%"; ;

                myReader = myCommand.ExecuteReader();
                table.Load(myReader); ;

                myReader.Close();
                myCon.Close();
            }
            return new JsonResult(table);
        }


        [HttpGet("getItemFromWarehouse")]
        public JsonResult GetItemFromWarehouse(string item_name)
        {
            string sqlDataSource = _configuration.GetConnectionString("warehouseCon");

            DataTable table = new();
            SqlDataReader myReader;

            string query = "SELECT ProductID, ProductName, ProductChName FROM Products " +
                    "WHERE ProductName LIKE @item_name";

            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new(query, myCon);
                myCommand.Parameters.Add("@item_name", SqlDbType.VarChar).Value = "%" + item_name + "%"; ;

                myReader = myCommand.ExecuteReader();
                table.Load(myReader); ;

                myReader.Close();
                myCon.Close();
            }
            return new JsonResult(table);
        }

        [HttpGet("getItemFromGroupDetails")]
        public JsonResult GetItemFromGroupDetails(int? ProduceGroupID = null)
        {
            string sqlDataSource = _configuration.GetConnectionString("aphqsvr3_pris23Con");

            DataTable table = new();
            SqlDataReader myReader;

            string query;
            if (ProduceGroupID.HasValue)
            {
                query = "SELECT produceGroupDetails.*, COALESCE(products.ProductName, item.item_desc_1) AS ProductName FROM ProduceGroupDetails produceGroupDetails " +
                    "LEFT JOIN [HQSVR2].[Warehouse].dbo.Products products ON produceGroupDetails.Item_Nbr = products.ProductID " +
                    "LEFT JOIN dbo.Item item ON produceGroupDetails.Item_Nbr = item.Item_Nbr " +
                    "WHERE ProduceGroupID = @ProduceGroupID";
            }
            else
            {
                query = "SELECT produceGroupDetails.*, COALESCE(products.ProductName, item.item_desc_1) AS ProductName FROM ProduceGroupDetails produceGroupDetails " +
                    "LEFT JOIN [HQSVR2].[Warehouse].dbo.Products products ON produceGroupDetails.Item_Nbr = products.ProductID " +
                    "LEFT JOIN dbo.Item item ON produceGroupDetails.Item_Nbr = item.Item_Nbr";
            }

            using (SqlConnection myCon = new(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new(query, myCon);

                if (ProduceGroupID.HasValue)
                {
                    myCommand.Parameters.Add("@ProduceGroupID", SqlDbType.Int).Value = ProduceGroupID.Value;
                }

                myReader = myCommand.ExecuteReader();
                table.Load(myReader); ;
                myReader.Close();
                myCon.Close();
            }
            return new JsonResult(table);
        }


        [HttpGet("getItemNotInDetails")]
        public JsonResult GetItemNotInDetails(int OrganizationID, string itemName)
        {
            var groupDetailsData = GetItemFromGroupDetails().Value as DataTable; // 同上
            // 获取ProduceGroupDetails数据
 
            // 构建一个HashSet来存储符合条件的Item_Nbr
            HashSet<int> validItemNbrs = new HashSet<int>();

            foreach (DataRow groupDetail in groupDetailsData.Rows)
            {
                int itemNbr = Convert.ToInt32(groupDetail["Item_Nbr"]);
                int? organizationId = groupDetail["OrganizationID"] as int?; // 假设这是可空的int类型

                if (organizationId != OrganizationID || organizationId == null)
                {
                    validItemNbrs.Add(itemNbr);
                }
            }

            DataTable productsData;
            DataTable filteredProducts;

            if (OrganizationID == 2)
            {
                productsData = GetItemByName(itemName).Value as DataTable; // 假设这会返回DataTable对象
                                                                            // 准备一个DataTable来存放过滤后的数据
                filteredProducts = productsData.Clone();
                foreach (DataRow product in productsData.Rows)
                {
                    int productId = Convert.ToInt32(product["Item_Nbr"]);
                    if (!groupDetailsData.AsEnumerable().Any(row => Convert.ToInt32(row["Item_Nbr"]) == productId))
                    {
                        filteredProducts.ImportRow(product);
                    }
                }
            }
            else
            {
                productsData = GetItemFromWarehouse(itemName).Value as DataTable; // 假设这会返回DataTable对象
                                                                                   // 准备一个DataTable来存放过滤后的数据
                filteredProducts = productsData.Clone();
                foreach (DataRow product in productsData.Rows)
                {
                    int productId = Convert.ToInt32(product["ProductID"]);
                    if (!groupDetailsData.AsEnumerable().Any(row => Convert.ToInt32(row["Item_Nbr"]) == productId))
                    {
                        filteredProducts.ImportRow(product);
                    }
                }
            }

            // 过滤Products数据
           

            return new JsonResult(filteredProducts);
        }


        [HttpPost("deleteGroupDetails")]
        public IActionResult DeleteGroupItem(GroupDetails groupDetails)
        {
            string sqlDataSource = _configuration.GetConnectionString("aphqsvr3_pris23Con");

            string query = "DELETE FROM dbo.ProduceGroupDetails " +
                "WHERE ProduceGroupID = @ProduceGroupID AND OrganizationID = @OrganizationID AND Item_Nbr = @Item_Nbr;";

            using SqlConnection myCon = new(sqlDataSource);
            myCon.Open();
            using SqlCommand myCommand = new(query, myCon);

            myCommand.Parameters.Add("@ProduceGroupID", SqlDbType.Int).Value = groupDetails.ProduceGroupID;
            myCommand.Parameters.Add("@OrganizationID", SqlDbType.Int).Value = groupDetails.OrganizationID;
            myCommand.Parameters.Add("@Item_Nbr", SqlDbType.Int).Value = groupDetails.Item_Nbr;

            int affectedRows = myCommand.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                return Ok(new { message = "Delete successful." });
            }
            else
            {
                return NotFound(new { message = "Group not found or no changes made." });
            }



        }


    }
}
