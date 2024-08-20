using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Domain.CommonModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.Interface.Services
{
    public interface IChartsService
    {
        Task<ResponseModel> GetRevenueDataAsync(string filter);
        Task<ResponseModel> GetMonthlyRevenueDataAsync();
        Task<ResponseModel> GetAudienceMetricsAsync(string filter);
        Task<ResponseModel> GetSessionsByCountriesAsync(string filter);
        Task<ResponseModel> GetBalanceOverviewAsync(int year);
        Task<ResponseModel> GetSalesByLocationsAsync();
        Task<ResponseModel> GetStoreVisitsBySourceAsync();
        Task<ResponseModel> AddOrderAsync(AddOrderDTO orderDto);
    }
}
