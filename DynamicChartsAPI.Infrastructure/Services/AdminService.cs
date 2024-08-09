using DynamicChartsAPI.Application.DTO_s;
using DynamicChartsAPI.Application.Interface.Repositories;
using DynamicChartsAPI.Application.Interface.Services;
using DynamicChartsAPI.Domain.CommonModal;
using DynamicChartsAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Infrastructure.Services
{
    // Application/Services/AdminService.cs
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _configuration;
        public AdminService(IAdminRepository adminRepository, IConfiguration configuration)
        {
            _adminRepository = adminRepository;
            _configuration = configuration;

        }
        public async Task<ResponseModel> AddUserAsync(AdminRegisterDto adminRegisterDto)
        {

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(adminRegisterDto.Password);

            var admin = new DgAdmin
            {
                Email = adminRegisterDto.Email,
                PasswordHash = hashedPassword
            };
            var userChanges = await _adminRepository.AddAsync(admin);
            if (userChanges > 0)
            {
                return new ResponseModel { StatusCode = 201, Message = "User added successfully" };
            }
            return new ResponseModel { StatusCode = 500, Message = "Failed to add user" };
        }

        public async Task<ResponseModel> LoginAsync(AdminLoginDto adminLoginDto)
        {
            var user = await _adminRepository.GetAdminByEmailAsync(adminLoginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(adminLoginDto.Password, user.PasswordHash))
            {
                return new ResponseModel { StatusCode = 401, Message = "Invalid credentials" };
            }
            var token = GenerateToken(user);
            return new ResponseModel { StatusCode = 200, Message = "Login successful", Data = new { Token = token } };
        }
        public string GenerateToken(DgAdmin dgAdmin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, dgAdmin.Id.ToString()),
                new Claim(ClaimTypes.Email, dgAdmin.Email),
            };
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(100),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
