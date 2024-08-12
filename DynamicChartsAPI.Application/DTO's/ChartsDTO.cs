using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.DTO_s
{
    public class RevenueDataDTO
    {
        public int TotalOrders { get; set; }
        public decimal TotalEarnings { get; set; }
        public int TotalRefunds { get; set; }
        public decimal ConversionRatio { get; set; }
    }
    public class MonthlyRevenueDataDTO
    {
        public List<string> Months { get; set; }
        public List<int> Orders { get; set; }
        public List<decimal> Earnings { get; set; }
        public List<int> Refunds { get; set; }
    }

    public class AudienceMetricsDTO
    {
        public int AvgSession { get; set; }
        public decimal ConversionRate { get; set; }
        public int AvgSessionDurationSeconds { get; set; }
        public int AvgSessionIncreasePercentage { get; set; }
        public int ConversionRateIncreasePercentage { get; set; }
        public int AvgSessionDurationIncreasePercentage { get; set; }
    }

    public class SessionsByCountriesDTO
    {
        public string CountryName { get; set; }
        public int Sessions { get; set; }
    }

    public class BalanceOverviewDTO
    {
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
        public decimal ProfitRatio { get; set; }
    }

    public class SalesByLocationsDTO
    {
        public string CountryName { get; set; }
        public decimal SalesPercentage { get; set; }
    }

    public class StoreVisitsBySourceDTO
    {
        public string SourceType { get; set; }
        public decimal Percentage { get; set; }
    }
}
