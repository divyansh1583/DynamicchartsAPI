using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.DTO_s
{
    public class SessionsByCountriesDTO
    {
        public List<CountrySessions> CountryData { get; set; }
    }

    public class CountrySessions
    {
        public string Country { get; set; }
        public decimal Sessions { get; set; }
    }
}
