using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.DTO_s
{
    public class BalanceOverviewDTO
    {
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
        public decimal ProfitRatio { get; set; }
        public List<MonthlyFinancials> MonthlyData { get; set; }
    }

    public class MonthlyFinancials
    {
        public int Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
    }
}
