using Dapper;
using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Application.Interface.Repositories;
using DynamicChartsAPI.Infrastructure.Data;
using System.Data;

namespace DynamicChartsAPI.Infrastructure.Repositories
{
    public class ChartsRepository : IChartsRepository
    {
        private readonly DapperContext _context;

        public ChartsRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<(int TotalOrders, decimal TotalEarnings, int TotalRefunds, decimal ConversionRatio)> GetRevenueDataAsync(string filter)
        {
            using var connection = _context.CreateConnection();

            var parameters = new { Filter = filter };

            var totalOrders = await connection.ExecuteScalarAsync<int>("EXEC usp_GetTotalOrders @Filter", parameters);
            var totalEarnings = await connection.ExecuteScalarAsync<decimal>("EXEC Usp_GetTotalEarnings @Filter", parameters);
            var totalRefunds = await connection.ExecuteScalarAsync<int>("EXEC usp_GetTotalRefund @Filter", parameters);
            var conversionRatio = await connection.ExecuteScalarAsync<decimal>("EXEC GetConversionRatio @Filter", parameters);

            return (totalOrders, totalEarnings, totalRefunds, conversionRatio);
        }

        public async Task<IEnumerable<(string Month, int Orders, decimal Earnings, int Refunds)>> GetMonthlyRevenueDataAsync()
        {
            using var connection = _context.CreateConnection();

            var query = @"
                SELECT 
                    DATENAME(MONTH, o.OrderDate) AS Month,
                    COUNT(DISTINCT o.OrderID) AS Orders,
                    SUM(p.SellingPrice * o.Quantity) AS Earnings,
                    COUNT(DISTINCT r.RefundID) AS Refunds
                FROM 
                    DCP_Order o
                    JOIN DCP_Product p ON o.ProductID = p.ProductID
                    LEFT JOIN DCP_Refund r ON o.OrderID = r.OrderID
                WHERE 
                    o.OrderDate >= DATEADD(YEAR, -1, GETDATE())
                GROUP BY 
                    DATENAME(MONTH, o.OrderDate), MONTH(o.OrderDate)
                ORDER BY 
                    MONTH(o.OrderDate)";

            var results = await connection.QueryAsync<(string Month, int Orders, decimal Earnings, int Refunds)>(query);
            return results;
        }

        public async Task<AudienceMetricsDTO> GetAudienceMetricsAsync(string filter)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryMultipleAsync("usp_GetAudienceMetrics", new { Filter = filter }, commandType: CommandType.StoredProcedure);

            var metrics = result.ReadFirstOrDefault<AudienceMetricsDTO>();
            if (metrics != null)
            {
                metrics.MonthlyData = result.Read<MonthlySessionData>().ToList();
            }
            return metrics;
        }

        public async Task<IEnumerable<SessionsByCountriesDTO>> GetSessionsByCountriesAsync(string filter)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SessionsByCountriesDTO>("usp_GetSessionsByCountries", new { Filter = filter }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<BalanceOverviewDTO> GetBalanceOverviewAsync(int year)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryMultipleAsync("usp_GetBalanceOverview", new { Year = year }, commandType: CommandType.StoredProcedure);

            var overview = result.ReadFirstOrDefault<BalanceOverviewDTO>();
            if (overview != null)
            {
                overview.MonthlyData = result.Read<MonthlyBalanceData>().ToList();
            }
            return overview;
        }

        public async Task<IEnumerable<SalesByLocationsDTO>> GetSalesByLocationsAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SalesByLocationsDTO>("usp_GetSalesByLocations", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<StoreVisitsBySourceDTO>> GetStoreVisitsBySourceAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<StoreVisitsBySourceDTO>("usp_GetStoreVisitsBySource", commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}