using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Application.Interface.Services;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<RevenueDataDTO>> GetRevenueData([FromQuery] string filter = "ALL")
        {
            var result = await _chartsService.GetRevenueDataAsync(filter);
            return Ok(result);
        }

        [HttpGet("monthly-revenue")]
        public async Task<ActionResult<MonthlyRevenueDataDTO>> GetMonthlyRevenueData()
        {
            var result = await _chartsService.GetMonthlyRevenueDataAsync();
            return Ok(result);
        }
        [HttpGet("audience-metrics")]
        public async Task<ActionResult<AudienceMetricsDTO>> GetAudienceMetrics([FromQuery] string filter = "ALL")
        {
            var result = await _chartsService.GetAudienceMetricsAsync(filter);
            return Ok(result);
        }

        [HttpGet("sessions-by-countries")]
        public async Task<ActionResult<IEnumerable<SessionsByCountriesDTO>>> GetSessionsByCountries([FromQuery] string filter = "ALL")
        {
            var result = await _chartsService.GetSessionsByCountriesAsync(filter);
            return Ok(result);
        }

        [HttpGet("balance-overview")]
        public async Task<ActionResult<BalanceOverviewDTO>> GetBalanceOverview([FromQuery] int year)
        {
            var result = await _chartsService.GetBalanceOverviewAsync(year);
            return Ok(result);
        }

        [HttpGet("sales-by-locations")]
        public async Task<ActionResult<IEnumerable<SalesByLocationsDTO>>> GetSalesByLocations()
        {
            var result = await _chartsService.GetSalesByLocationsAsync();
            return Ok(result);
        }

        [HttpGet("store-visits-by-source")]
        public async Task<ActionResult<IEnumerable<StoreVisitsBySourceDTO>>> GetStoreVisitsBySource()
        {
            var result = await _chartsService.GetStoreVisitsBySourceAsync();
            return Ok(result);
        }
    }
}

