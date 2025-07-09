using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyRepository(ApplicationDbContext dbContext) : GenericRepository<Property>(dbContext), IPropertyRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<Property>> GetPropertiesByAgentIdAsync(string agentId)
        {
            return await _dbContext.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .Include(p => p.Improvements)
                .Where(p => p.UserId == agentId)
                .ToListAsync();
        }

        public async Task<List<Property>> GetAvailablePropertiesAsync()
        {
            return await _dbContext.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .Where(p => p.PropertyStatus == PropertyStatus.Available.ToString())
                .ToListAsync();
        }


        public override async Task<Property> GetByIdAsync(int id)
        {
            return await _dbContext.Properties
                .Include(p => p.Improvements)
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetPropertyCountByAgentId(string agentId)
            => await _dbContext.Properties.Where(p => p.UserId == agentId).CountAsync();
    }
}
