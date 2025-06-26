//ChartOfAccountRepository.cs
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MiniAccountManagementSystem.Models;
using System.Data;

namespace MiniAccountManagementSystem.Data
{
    public class ChartOfAccountRepository
    {
        private readonly IConfiguration _configuration;

        public ChartOfAccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string ConnectionString => _configuration.GetConnectionString("DefaultConnection");

        public List<ChartOfAccount> GetAccounts()
        {
            var accounts = new List<ChartOfAccount>();
            using var conn = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("SELECT A.Id, A.Name, A.ParentId, P.Name AS ParentName FROM ChartOfAccounts A LEFT JOIN ChartOfAccounts P ON A.ParentId = P.Id", conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accounts.Add(new ChartOfAccount
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    ParentId = reader["ParentId"] as int?,
                    ParentName = reader["ParentName"]?.ToString()
                });
            }
            return accounts;
        }

        public void Save(ChartOfAccount account, string action)
        {
            using var conn = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("sp_ManageChartOfAccounts", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@Id", account.Id);
            cmd.Parameters.AddWithValue("@Name", account.Name);
            cmd.Parameters.AddWithValue("@ParentId", (object)account.ParentId ?? DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public ChartOfAccount GetById(int id)
        {
            using var conn = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("SELECT * FROM ChartOfAccounts WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ChartOfAccount
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    ParentId = reader["ParentId"] as int?
                };
            }
            return null;
        }
    }
}
