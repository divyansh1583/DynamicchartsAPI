using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.DTO_s
{
    public class StoreVisitsBySourceDTO
    {
        public List<SourceVisits> SourceData { get; set; }
    }

    public class SourceVisits
    {
        public string Source { get; set; }
        public decimal Percentage { get; set; }
    }
}
