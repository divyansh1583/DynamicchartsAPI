using DynamicChartsAPI.Application.DTO_s;
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
        Task<AudienceMetricsDTO> GetAudienceMetricsAsync(string filter);
        Task<IEnumerable<SessionsByCountriesDTO>> GetSessionsByCountriesAsync(string filter);
        Task<BalanceOverviewDTO> GetBalanceOverviewAsync(int year);
        Task<IEnumerable<SalesByLocationsDTO>> GetSalesByLocationsAsync();
        Task<IEnumerable<StoreVisitsBySourceDTO>> GetStoreVisitsBySourceAsync();
    }
}
