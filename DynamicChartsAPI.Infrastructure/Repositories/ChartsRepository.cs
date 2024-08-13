using Dapper;
using DynamicChartsAPI.Application.Interface.Repositories;
using DynamicChartsAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<AudienceMetricsDto> GetAudienceMetricsAsync(string filter)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<AudienceMetricsDto>("usp_GetAudienceMetrics", new { Filter = filter }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<SessionsByCountryDto>> GetSessionsByCountriesAsync(string filter)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SessionsByCountryDto>("usp_GetSessionsByCountries", new { Filter = filter }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<BalanceOverviewDto> GetBalanceOverviewAsync(int year)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<BalanceOverviewDto>("usp_GetBalanceOverview", new { Year = year }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<SalesByLocationDto>> GetSalesByLocationsAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SalesByLocationDto>("usp_GetSalesByLocations", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<StoreVisitsBySourceDto>> GetStoreVisitsBySourceAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<StoreVisitsBySourceDto>("usp_GetStoreVisitsBySource", commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
