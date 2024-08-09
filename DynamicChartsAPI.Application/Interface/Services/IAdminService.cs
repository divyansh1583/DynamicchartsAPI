using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Domain.CommonModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.Interface.Services
{
    public interface IAdminService
    {
        Task<ResponseModel> AddUserAsync(AdminRegisterDto adminRegisterDto);
        Task<ResponseModel> LoginAsync(AdminLoginDto adminLoginDto);
    }
}
