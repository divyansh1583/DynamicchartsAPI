using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Application.Interface.Services;
using DynamicChartsAPI.Domain.CommonModal;
using Microsoft.AspNetCore.Mvc;

namespace DynamicChartsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly IChartsService _chartsService;

        public ChartsController(IChartsService chartsService)
        {
            _chartsService = chartsService;
        }

        [HttpGet("revenue")]
        public async Task<ActionResult<ResponseModel>> GetRevenueData([FromQuery] string filter = "ALL")
        {
            var result = await _chartsService.GetRevenueDataAsync(filter);
            return Ok(result);
        }

        [HttpGet("monthly-revenue")]
        public async Task<ActionResult<ResponseModel>> GetMonthlyRevenueData()
        {
            var result = await _chartsService.GetMonthlyRevenueDataAsync();
            return Ok(result);
        }

        [HttpGet("audience-metrics")]
        public async Task<ActionResult<ResponseModel>> GetAudienceMetrics([FromQuery] string filter = "ALL")
        {
            var result = await _chartsService.GetAudienceMetricsAsync(filter);
            return Ok(result);
        }

        [HttpGet("sessions-by-countries")]
        public async Task<ActionResult<ResponseModel>> GetSessionsByCountries([FromQuery] string filter = "ALL")
        {
            var result = await _chartsService.GetSessionsByCountriesAsync(filter);
            return Ok(result);
        }

        [HttpGet("balance-overview")]
        public async Task<ActionResult<ResponseModel>> GetBalanceOverview([FromQuery] int year)
        {
            var result = await _chartsService.GetBalanceOverviewAsync(year);
            return Ok(result);
        }

        [HttpGet("sales-by-locations")]
        public async Task<ActionResult<ResponseModel>> GetSalesByLocations()
        {
            var result = await _chartsService.GetSalesByLocationsAsync();
            return Ok(result);
        }

        [HttpGet("store-visits-by-source")]
        public async Task<ActionResult<ResponseModel>> GetStoreVisitsBySource()
        {
            var result = await _chartsService.GetStoreVisitsBySourceAsync();
            return Ok(result);
        }
        [HttpPost("add-order")]
        public async Task<ActionResult<ResponseModel>> AddOrder([FromBody] AddOrderDTO orderDto)
        {
            var result = await _chartsService.AddOrderAsync(orderDto);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}