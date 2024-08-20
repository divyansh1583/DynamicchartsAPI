using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.DTO_s
{
    public class AddOrderDTO
    {
        public int ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public int SourceId { get; set; }
        public int CountryId { get; set; }
    }
}
