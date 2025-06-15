using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmSystem.Core.Models;

namespace FarmSystem.Core.Interfaces
{
    public interface ICowRepository
{
    Task<IEnumerable<Cow>> GetAllAsync();
    Task<Cow?> GetByIdAsync(int id);
    Task AddAsync(Cow cow);
    Task UpdateAsync(Cow cow);
    Task DeleteAsync(int id);
}

    
}