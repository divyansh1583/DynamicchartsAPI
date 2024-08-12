using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.DTO_s
{
    public class AudienceMetricsDTO
    {
        public decimal AvgSession { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal AvgSessionDurationSeconds { get; set; }
        public decimal AvgSessionIncreasePercentage { get; set; }
        public decimal ConversionRateIncreasePercentage { get; set; }
        public decimal AvgSessionDurationIncreasePercentage { get; set; }
        public List<MonthlySessions> MonthlyData { get; set; }
    }

    public class MonthlySessions
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Sessions { get; set; }
    }
}
