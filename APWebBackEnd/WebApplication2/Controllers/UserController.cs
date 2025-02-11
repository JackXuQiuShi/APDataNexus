using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using APWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Azure.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace APWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly string sqlDataSource = configuration.GetConnectionString("pris23Con");
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        [Authorize(AuthenticationSchemes = "Windows")]
        [HttpGet("domainLogin")]
        public async Task<ActionResult<User>> DomainLogin()
        {
            var identity = HttpContext.User.Identity;

            // 检查用户是否已认证
            if (identity != null && identity.IsAuthenticated)
            {
                int indexOfBackslash = identity.Name.IndexOf("\\");
                string name = identity.Name.Substring(indexOfBackslash + 1);

                var user = await GetUserAsync(name);

                if (user == null)
                {
                    await HttpContext.ChallengeAsync("Windows");
                    return Unauthorized("User is not authenticated.");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(10),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString, user.Name, user.Store, user.Role });
            }
            else
            {
                return Unauthorized("User is not authenticated.");
            }
        }

        [HttpGet("getUser")]
        public async Task<User> GetUserAsync(string Name)
        {
            string query = "SELECT Name, Store, ID, STRING_AGG(Role, ',') AS Role " +
                "FROM dbo.UserInfo WHERE Name = @Name GROUP BY Name, Store, ID;";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", SqlDbType.VarChar) { Value = Name }
            };

            DataTable result = await ExecuteQueryAsync(query, parameters);

            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                return new User
                {
                    Name = row["Name"].ToString(),
                    Store = Convert.ToInt32(row["Store"]),
                    Role = row["Role"].ToString(),
                    ID = Convert.ToInt32(row["ID"])
                };
            }
            return null;
        }


        /* [HttpGet("getCurrentUser")]
         public IActionResult GetCurrentUser()
         {
             //var identity = HttpContext.User.Identity;
             var user = GetUser();
             // 使用 Ok() 方法返回200 OK响应，并包含用户名

             return Ok(user);

         }
         [HttpGet("getUser")]
         public User GetUser()
         {
             return new User
             {
                 Name = "storeUser",
                 Store = 35,
                 Role = "store",
                 ID = 100
             };
         }*/
        [HttpPost("accountLogin")]
        public async Task<IActionResult> AccountLogin([FromBody] User request)
        {
            string query = "SELECT PasswordHash FROM UserAccount WHERE Username = @username";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", SqlDbType.VarChar) { Value = request.Username }
            };

            var storedHash = (string)await ExecuteScalarAsync(query, parameters);

            if (storedHash == null)
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }

            var result = _passwordHasher.VerifyHashedPassword(new User { PasswordHash = storedHash }, storedHash, request.PasswordHash);
            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }

            var userInfo = await GetUserAsync(request.Username);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInfo.Name),
                    new Claim(ClaimTypes.Role, userInfo.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString, userInfo.Name, userInfo.Store, userInfo.Role });
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Models.RegisterRequest request)
        {
            string queryCheckUser = "SELECT COUNT(1) FROM UserAccount WHERE Username = @username";

            SqlParameter[] parametersCheck = new SqlParameter[]
            {
                new SqlParameter("@username", SqlDbType.VarChar) { Value = request.Username }
            };

            var userExists = (int)await ExecuteScalarAsync(queryCheckUser, parametersCheck) > 0;

            if (userExists)
            {
                return Conflict(new { Message = "Username already exists." });
            }

            var user = new User
            {
                Username = request.Username,
                PasswordHash = _passwordHasher.HashPassword(null, request.Password)
            };

            string queryInsertUser = "INSERT INTO UserAccount (Username, PasswordHash) VALUES (@username, @passwordHash)";

            SqlParameter[] parametersInsert = new SqlParameter[]
            {
                new SqlParameter("@username", SqlDbType.VarChar) { Value = user.Username },
                new SqlParameter("@passwordHash", SqlDbType.VarChar) { Value = user.PasswordHash }
            };

            await ExecuteNonQueryAsync(queryInsertUser, parametersInsert);

            return Ok(new { Message = "User registered successfully" });
        }











     /*   [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Optionally, you can also clear the cookies manually if needed
            Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to a different page or return a response indicating successful logout
            return Ok(new { Message = "User logged out successfully." });
        }*/




/*        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(".AspNetCore.Cookies");

            // 返回一个要求客户端重新认证的响应
            return Unauthorized();
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







    }
}
