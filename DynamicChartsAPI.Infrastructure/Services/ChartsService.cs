using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Application.Interface.Repositories;
using DynamicChartsAPI.Application.Interface.Services;
using DynamicChartsAPI.Domain.CommonModal;

namespace DynamicChartsAPI.Infrastructure.Services
{
    public class ChartsService : IChartsService
    {
        private readonly IChartsRepository _chartsRepository;

        public ChartsService(IChartsRepository chartsRepository)
        {
            _chartsRepository = chartsRepository;
        }

        public async Task<ResponseModel> GetRevenueDataAsync(string filter)
        {
            var (totalOrders, totalEarnings, totalRefunds, conversionRatio) = await _chartsRepository.GetRevenueDataAsync(filter);

            var revenueData = new RevenueDataDTO
            {
                TotalOrders = totalOrders,
                TotalEarnings = totalEarnings,
                TotalRefunds = totalRefunds,
                ConversionRatio = conversionRatio
            };

            return new ResponseModel { StatusCode = 200, Data = revenueData };
        }

        public async Task<ResponseModel> GetMonthlyRevenueDataAsync()
        {
            var data = await _chartsRepository.GetMonthlyRevenueDataAsync();

            var monthlyRevenueData = new MonthlyRevenueDataDTO
            {
                Months = data.Select(d => d.Month).ToList(),
                Orders = data.Select(d => d.Orders).ToList(),
                Earnings = data.Select(d => d.Earnings).ToList(),
                Refunds = data.Select(d => d.Refunds).ToList()
            };

            return new ResponseModel { StatusCode = 200, Data = monthlyRevenueData };
        }

        public async Task<ResponseModel> GetAudienceMetricsAsync(string filter)
        {
            var result = await _chartsRepository.GetAudienceMetricsAsync(filter);
            return new ResponseModel { StatusCode = 200, Data = result };
        }

        public async Task<ResponseModel> GetSessionsByCountriesAsync(string filter)
        {
            var result = await _chartsRepository.GetSessionsByCountriesAsync(filter);
            return new ResponseModel { StatusCode = 200, Data = result };
        }

        public async Task<ResponseModel> GetBalanceOverviewAsync(int year)
        {
            var result = await _chartsRepository.GetBalanceOverviewAsync(year);
            if (result == null)
            {
                return new ResponseModel { StatusCode = 404, Message = $"No balance overview data found for year {year}." };
            }
            return new ResponseModel { StatusCode = 200, Data = result };
        }

        public async Task<ResponseModel> GetSalesByLocationsAsync()
        {
            var result = await _chartsRepository.GetSalesByLocationsAsync();
            return new ResponseModel { StatusCode = 200, Data = result };
        }

        public async Task<ResponseModel> GetStoreVisitsBySourceAsync()
        {
            var result = await _chartsRepository.GetStoreVisitsBySourceAsync();
            return new ResponseModel { StatusCode = 200, Data = result };
        }
        public async Task<ResponseModel> AddOrderAsync(AddOrderDTO orderDto)
        {
            try
            {
                var (newProductId, newOrderId) = await _chartsRepository.AddOrderAsync(
                    orderDto.ProductId,
                    orderDto.OrderDate,
                    orderDto.Quantity,
                    orderDto.SourceId,
                    orderDto.CountryId
                );

                var result = new
                {
                    NewProductId = newProductId,
                    NewOrderId = newOrderId
                };

                return new ResponseModel { StatusCode = 200, Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while adding the order: {ex.Message}" };
            }
        }
    }
}