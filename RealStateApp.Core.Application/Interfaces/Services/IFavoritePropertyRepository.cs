using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IFavoritePropertyRepository : IGenericRepository<FavoriteProperty>
    {
        Task<List<FavoriteProperty>> GetByUserIdAsync(string userId);
        Task<FavoriteProperty> GetByUserIdAndPropertyIdAsync(string userId, int propertyId);
    }
}
