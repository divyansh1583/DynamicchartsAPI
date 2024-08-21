using DynamicChartsAPI.Application.Interface.Repositories;
using DynamicChartsAPI.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace DynamicChartsAPI.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _connectionString;

        public AdminRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> AddAsync(DgAdmin admin)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            INSERT INTO DgAdmin (Email, PasswordHash)
            VALUES (@Email, @PasswordHash);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            return await connection.ExecuteScalarAsync<int>(sql, admin);
        }

        public async Task<DgAdmin> GetAdminByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT * FROM DgAdmin WHERE Email = @Email";

            return await connection.QueryFirstOrDefaultAsync<DgAdmin>(sql, new { Email = email });
        }
    }
}
