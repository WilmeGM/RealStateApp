using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class FavoritePropertyRepository(ApplicationDbContext dbContext) : GenericRepository<FavoriteProperty>(dbContext), IFavoritePropertyRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<FavoriteProperty>> GetByUserIdAsync(string userId)
        {
            return await _dbContext.FavoriteProperties.Where(fp => fp.UserId == userId).ToListAsync();
        }

        public async Task<FavoriteProperty> GetByUserIdAndPropertyIdAsync(string userId, int propertyId)
        {
            return await _dbContext.FavoriteProperties.FirstOrDefaultAsync(fp => fp.UserId == userId && fp.PropertyId == propertyId);
        }
    }
}
