using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Application.Interface.Services;
using DynamicChartsAPI.Domain.CommonModal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicChartsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<ResponseModel>> AddUser([FromBody] AdminRegisterDto adminRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel { StatusCode = 400, Message = "Invalid request", Data = ModelState });
            }
            return await _adminService.AddUserAsync(adminRegisterDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel>> Login(AdminLoginDto adminLoginDto)
        {
            return await _adminService.LoginAsync(adminLoginDto);
        }
    }
}
