using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.DTO_s
{
    public class AddDetailsDTO
    {
        public string? CountryName { get; set; }
        public string? SourceType { get; set; }
        public string? ProductName { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? Quantity { get; set; }
        public DateTime? RefundDate { get; set; }
        public DateTime? SessionStartDate { get; set; }
        public DateTime? SessionEndDate { get; set; }
    }
    public class AddDetailsResultDTO
    {
        public int? CountryID { get; set; }
        public int? SourceID { get; set; }
        public int? ProductID { get; set; }
        public int? OrderID { get; set; }
    }
}
