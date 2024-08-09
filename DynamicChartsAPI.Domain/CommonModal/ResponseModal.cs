using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Domain.CommonModal
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public object? Data { get; set; }
        public string Message { get; set; }
    }
}
