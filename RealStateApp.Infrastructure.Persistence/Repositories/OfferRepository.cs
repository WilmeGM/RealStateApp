using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class OfferRepository(ApplicationDbContext dbContext) : GenericRepository<Offer>(dbContext), IOfferRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<Offer>> GetOffersByPropertyAndUserAsync(int propertyId, string userId)
        {
            return await _dbContext.Offers
                .Where(o => o.PropertyId == propertyId && o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetOffersByPropertyAsync(int propertyId)
        {
            return await _dbContext.Offers
                .Where(o => o.PropertyId == propertyId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
