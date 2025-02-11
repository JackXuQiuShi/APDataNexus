using Microsoft.AspNetCore.Mvc;

namespace APWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        /*
        private readonly IConfiguration _configuration;


        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpGet("GetItem")]
        public JsonResult GetItem()
        {
            string sqlDataSource = _configuration.GetConnectionString("prisCon");
            DataTable table = new DataTable();
            SqlDataReader myReader;
            string query;


            query = @"  
               select * from dbo.Categories;";

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }


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
    }
}
