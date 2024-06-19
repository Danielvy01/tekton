using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Tekton.Configuration.Infraestructure.Data
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
    }
    [ExcludeFromCodeCoverage]
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection
        {
            get
            {
                string? passwordsql = _configuration["ConexionSQL:Password"];// Environment.GetEnvironmentVariable("PASSWORD_SQL");

                var sqlConnection = new SqlConnection();
                if (passwordsql != null)
                {
                    sqlConnection.ConnectionString = $"Data Source={_configuration["ConexionSQL:Server"]};Initial Catalog={_configuration["ConexionSQL:Database"]};Persist Security Info=False;Pwd={passwordsql};User ID={_configuration["ConexionSQL:UserName"]}";
                    sqlConnection.Open();
                }
                return sqlConnection;
            }
        }
    }
}
