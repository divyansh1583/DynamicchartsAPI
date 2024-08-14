using DynamicChartsAPI.Application.Interface.Repositories;
using DynamicChartsAPI.Application.Interface.Services;
using DynamicChartsAPI.Infrastructure.Repositories;
using DynamicChartsAPI.Infrastructure.Services;
using DynamicChartsAPI.Infrastructure.Data;
using Microsoft.OpenApi.Models;

namespace DynamicChartsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dynamic Charts API", Version = "v1" });
            });

            // Register DapperContext
            builder.Services.AddSingleton<DapperContext>();

            // Register repositories
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IChartsRepository, ChartsRepository>();

            // Register services
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IChartsService, ChartsService>();

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dynamic Charts API v1"));
            }

            app.UseHttpsRedirection();

            // Use CORS policy
            app.UseCors("AllowAnyOrigin");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}