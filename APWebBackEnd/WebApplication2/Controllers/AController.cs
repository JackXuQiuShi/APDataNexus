using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace APWeb.Controllers
{
    public class AlpController : Controller
    {
        private readonly IConfiguration _configuration;

        public AlpController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpGet("{id}")]
        public JsonResult Get(string id, string startDt, string endDt)
        {
            string sqlDataSource = _configuration.GetConnectionString("AlpCon");
            DataTable table = new DataTable();
            SqlDataReader myReader;
            string query;

            if (startDt == null && endDt == null)
            {
                query = @"  
                    select a.Product_ID, c.Prod_Name, b.RegPrice, 
                    CONVERT(DECIMAL(10,2),a.Quantity) as Quantity, CONVERT(DECIMAL(10,2),a.Amount) as Amount, 
                    CONVERT(varchar, a.Date,23) as Date from dbo.POS_Sales a 
                    join dbo.ProductPrice b on a.Product_ID = b.ProdNum 
                    join dbo.Products c on a.Product_ID = c.Prod_Num
                    where Product_ID like '%" + id + @"' 
                    order by Date desc";
            }
            else
            {
                query = @"  
                    select a.Product_ID, c.Prod_Name, b.RegPrice, 
                    CONVERT(DECIMAL(10,2),a.Quantity) as Quantity, CONVERT(DECIMAL(10,2),a.Amount) as Amount, 
                    CONVERT(varchar, a.Date,23) as Date from dbo.POS_Sales a 
                    join dbo.ProductPrice b on a.Product_ID = b.ProdNum 
                    join dbo.Products c on a.Product_ID = c.Prod_Num
                    where Product_ID like '%" + id + @"' and Date between '" + startDt + @"' and '" + endDt + @"'
                    order by Date desc";
            }

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
    }
}
