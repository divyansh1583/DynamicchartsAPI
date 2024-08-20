using Dapper;
using DynamicChartsAPI.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace DynamicChartsAPI.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _connectionString;
        private readonly DapperContext _context;

        public ErrorLoggingMiddleware(RequestDelegate next, IConfiguration configuration, DapperContext context)
        {
            _next = next;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogErrorToDatabase(ex, context);
                throw;
            }
        }

        private async Task LogErrorToDatabase(Exception ex, HttpContext context)
        {
            using var connection = _context.CreateConnection();

            var query = @"
                INSERT INTO ErrorLogs (Timestamp, Message, StackTrace, RequestPath, RequestMethod)
                VALUES (@Timestamp, @Message, @StackTrace, @RequestPath, @RequestMethod)";

            var parameters = new
            {
                Timestamp = DateTime.UtcNow,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                RequestPath = context.Request.Path.Value,
                RequestMethod = context.Request.Method
            };

            await connection.ExecuteAsync(query, parameters);
        }
    }
}
