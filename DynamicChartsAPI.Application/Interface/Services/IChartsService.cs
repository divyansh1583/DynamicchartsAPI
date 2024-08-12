using DynamicChartsAPI.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.Interface.Services
{
    public interface IChartsService
    {
        Task<RevenueDataDTO> GetRevenueDataAsync(string filter);
        Task<MonthlyRevenueDataDTO> GetMonthlyRevenueDataAsync();
        Task<AudienceMetricsDTO> GetAudienceMetricsAsync(string filter);
        Task<IEnumerable<SessionsByCountriesDTO>> GetSessionsByCountriesAsync(string filter);
        Task<BalanceOverviewDTO> GetBalanceOverviewAsync(int year);
        Task<IEnumerable<SalesByLocationsDTO>> GetSalesByLocationsAsync();
        Task<IEnumerable<StoreVisitsBySourceDTO>> GetStoreVisitsBySourceAsync();
    }
}
