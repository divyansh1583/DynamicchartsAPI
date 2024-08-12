using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.Interface.Repositories
{
    public interface IChartsRepository
    {
        Task<(int TotalOrders, decimal TotalEarnings, int TotalRefunds, decimal ConversionRatio)> GetRevenueDataAsync(string filter);
        Task<IEnumerable<(string Month, int Orders, decimal Earnings, int Refunds)>> GetMonthlyRevenueDataAsync();
        Task<AudienceMetricsDto> GetAudienceMetricsAsync(string filter);
        Task<IEnumerable<SessionsByCountryDto>> GetSessionsByCountriesAsync(string filter);
        Task<BalanceOverviewDto> GetBalanceOverviewAsync(int year);
        Task<IEnumerable<SalesByLocationDto>> GetSalesByLocationsAsync();
        Task<IEnumerable<StoreVisitsBySourceDto>> GetStoreVisitsBySourceAsync();
    }
}
