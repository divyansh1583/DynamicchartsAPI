using DynamicChartsAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicChartsAPI.Application.Interface.Repositories
{
    public interface IAdminRepository
    {
        Task<int> AddAsync(DgAdmin user);
        Task<DgAdmin> GetAdminByEmailAsync(string email);
    }
}
