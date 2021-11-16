using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQLWebApp.Services
{
    public class DbAuthConnector
    {
        private readonly ITokenAcquisition _tokenAcquirer;
        private readonly IConfiguration _configuration;
        public DbAuthConnector(IConfiguration conf, ITokenAcquisition tokenAcquirer)
        {
            _configuration = conf;
            _tokenAcquirer = tokenAcquirer;
        }
        public async Task<SqlConnection> Connect(string confKey)
        {
            var token = await _tokenAcquirer.GetAccessTokenForUserAsync(new string[] { "https://database.windows.net//user_impersonation" });
            using var connection = new SqlConnection(_configuration.GetConnectionString(confKey))
            {
                AccessToken = token
            };
            await connection.OpenAsync();
            return connection;
        }
    }
}
