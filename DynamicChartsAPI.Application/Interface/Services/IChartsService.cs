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
        Task<IEnumerable<SessionsByCountryDTO>> GetSessionsByCountriesAsync(string filter);
        Task<BalanceOverviewDTO> GetBalanceOverviewAsync(int year);
        Task<IEnumerable<SalesByLocationDTO>> GetSalesByLocationsAsync();
        Task<IEnumerable<StoreVisitsBySourceDTO>> GetStoreVisitsBySourceAsync();
    }
}
