using System;
using System.Collections.Generic;
using Xunit;
using DynamicChartsAPI.Application.DTO_s;

namespace DynamicChartAPI.Test.DTOs
{
    public class DynamicChartsDTOTests
    {
        [Fact]
        public void RevenueDataDTO_PropertiesSetAndGet_Correctly()
        {
            var dto = new RevenueDataDTO
            {
                TotalOrders = 100,
                TotalEarnings = 1000.50m,
                TotalRefunds = 5,
                ConversionRatio = 0.75m
            };

            Assert.Equal(100, dto.TotalOrders);
            Assert.Equal(1000.50m, dto.TotalEarnings);
            Assert.Equal(5, dto.TotalRefunds);
            Assert.Equal(0.75m, dto.ConversionRatio);
        }

        [Fact]
        public void MonthlyRevenueDataDTO_PropertiesSetAndGet_Correctly()
        {
            var dto = new MonthlyRevenueDataDTO
            {
                Months = new List<string> { "Jan", "Feb", "Mar" },
                Orders = new List<int> { 10, 20, 30 },
                Earnings = new List<decimal> { 100.5m, 200.5m, 300.5m },
                Refunds = new List<int> { 1, 2, 3 }
            };

            Assert.Equal(3, dto.Months.Count);
            Assert.Equal("Feb", dto.Months[1]);
            Assert.Equal(20, dto.Orders[1]);
            Assert.Equal(300.5m, dto.Earnings[2]);
            Assert.Equal(1, dto.Refunds[0]);
        }

        [Fact]
        public void AudienceMetricsDTO_PropertiesSetAndGet_Correctly()
        {
            var dto = new AudienceMetricsDTO
            {
                Avg_Session = 5,
                Conversion_Rate = 0.25m,
                Avg_Session_Duration_Seconds = 300,
                Avg_Session_Increase_Percentage = 0.1m,
                Conversion_Rate_Increase_Percentage = 0.05m,
                Avg_Session_Duration_Increase_Percentage = 0.15m,
                MonthlyData = new List<MonthlySessionData>
                {
                    new MonthlySessionData { Month = 1, Sessions = 100, Year = 2024 }
                }
            };

            Assert.Equal(5, dto.Avg_Session);
            Assert.Equal(0.25m, dto.Conversion_Rate);
            Assert.Equal(300, dto.Avg_Session_Duration_Seconds);
            Assert.Equal(0.1m, dto.Avg_Session_Increase_Percentage);
            Assert.Equal(0.05m, dto.Conversion_Rate_Increase_Percentage);
            Assert.Equal(0.15m, dto.Avg_Session_Duration_Increase_Percentage);
            Assert.Single(dto.MonthlyData);
            Assert.Equal(2024, dto.MonthlyData[0].Year);
        }

        [Fact]
        public void SessionsByCountriesDTO_PropertiesSetAndGet_Correctly()
        {
            var dto = new SessionsByCountriesDTO
            {
                CountryName = "USA",
                Sessions = 1000
            };

            Assert.Equal("USA", dto.CountryName);
            Assert.Equal(1000, dto.Sessions);
        }

        [Fact]
        public void BalanceOverviewDTO_PropertiesSetAndGet_Correctly()
        {
            var dto = new BalanceOverviewDTO
            {
                Revenue = 10000m,
                Expenses = 5000m,
                ProfitRatio = 0.5m,
                MonthlyData = new List<MonthlyBalanceData>
                {
                    new MonthlyBalanceData { MonthName = "January", Revenue = 1000m, Expenses = 500m }
                }
            };

            Assert.Equal(10000m, dto.Revenue);
            Assert.Equal(5000m, dto.Expenses);
            Assert.Equal(0.5m, dto.ProfitRatio);
            Assert.Single(dto.MonthlyData);
            Assert.Equal("January", dto.MonthlyData[0].MonthName);
        }

        [Fact]
        public void SalesByLocationsDTO_PropertiesSetAndGet_Correctly()
        {
            var dto = new SalesByLocationsDTO
            {
                CountryName = "Canada",
                SalesPercentage = 0.15m
            };

            Assert.Equal("Canada", dto.CountryName);
            Assert.Equal(0.15m, dto.SalesPercentage);
        }

        [Fact]
        public void StoreVisitsBySourceDTO_PropertiesSetAndGet_Correctly()
        {
            var dto = new StoreVisitsBySourceDTO
            {
                SourceType = "Direct",
                Percentage = 0.3m
            };

            Assert.Equal("Direct", dto.SourceType);
            Assert.Equal(0.3m, dto.Percentage);
        }
    }
}