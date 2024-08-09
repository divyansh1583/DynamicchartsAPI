using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.DTO_s
{
    public class AdminRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AdminLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
