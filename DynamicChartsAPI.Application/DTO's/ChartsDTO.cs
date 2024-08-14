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
        public int Avg_Session { get; set; }
        public decimal Conversion_Rate { get; set; }
        public int Avg_Session_Duration_Seconds { get; set; }
        public decimal Avg_Session_Increase_Percentage { get; set; }
        public decimal Conversion_Rate_Increase_Percentage { get; set; }
        public decimal Avg_Session_Duration_Increase_Percentage { get; set; }
        public List<MonthlySessionData> MonthlyData { get; set; }
    }

    public class MonthlySessionData
    {
        public int Month { get; set; }
        public int Sessions { get; set; }
        public int Year { get; set; }
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
        public List<MonthlyBalanceData> MonthlyData { get; set; }
    }


    public class MonthlyBalanceData
    {
        public string MonthName { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
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
