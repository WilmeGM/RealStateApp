using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IOfferRepository : IGenericRepository<Offer>
    {
        Task<List<Offer>> GetOffersByPropertyAndUserAsync(int propertyId, string userId);
        Task<List<Offer>> GetOffersByPropertyAsync(int propertyId);
    }
}
