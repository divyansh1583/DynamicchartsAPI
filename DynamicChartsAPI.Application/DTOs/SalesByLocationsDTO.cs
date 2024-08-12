using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.DTO_s
{
    public class SalesByLocationsDTO
    {
        public List<LocationSales> LocationData { get; set; }
    }

    public class LocationSales
    {
        public string Location { get; set; }
        public decimal SalesPercentage { get; set; }
    }
}
