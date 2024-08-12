using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Application.Interface.Repositories;
using DynamicChartsAPI.Application.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Infrastructure.Services
{
    public class ChartsService : IChartsService
    {
        private readonly IChartsRepository _chartsRepository;

        public ChartsService(IChartsRepository chartsRepository)
        {
            _chartsRepository = chartsRepository;
        }

        public async Task<RevenueDataDTO> GetRevenueDataAsync(string filter)
        {
            var (totalOrders, totalEarnings, totalRefunds, conversionRatio) = await _chartsRepository.GetRevenueDataAsync(filter);

            return new RevenueDataDTO
            {
                TotalOrders = totalOrders,
                TotalEarnings = totalEarnings,
                TotalRefunds = totalRefunds,
                ConversionRatio = conversionRatio
            };
        }

        public async Task<MonthlyRevenueDataDTO> GetMonthlyRevenueDataAsync()
        {
            var data = await _chartsRepository.GetMonthlyRevenueDataAsync();

            return new MonthlyRevenueDataDTO
            {
                Months = data.Select(d => d.Month).ToList(),
                Orders = data.Select(d => d.Orders).ToList(),
                Earnings = data.Select(d => d.Earnings).ToList(),
                Refunds = data.Select(d => d.Refunds).ToList()
            };
        }
        public async Task<AudienceMetricsDTO> GetAudienceMetricsAsync(string filter)
        {
            return await _chartsRepository.GetAudienceMetricsAsync(filter);
        }

        public async Task<IEnumerable<SessionsByCountriesDTO>> GetSessionsByCountriesAsync(string filter)
        {
            return await _chartsRepository.GetSessionsByCountriesAsync(filter);
        }

        public async Task<BalanceOverviewDTO> GetBalanceOverviewAsync(int year)
        {
            return await _chartsRepository.GetBalanceOverviewAsync(year);
        }

        public async Task<IEnumerable<SalesByLocationsDTO>> GetSalesByLocationsAsync()
        {
            return await _chartsRepository.GetSalesByLocationsAsync();
        }

        public async Task<IEnumerable<StoreVisitsBySourceDTO>> GetStoreVisitsBySourceAsync()
        {
            return await _chartsRepository.GetStoreVisitsBySourceAsync();
        }
    }
}

