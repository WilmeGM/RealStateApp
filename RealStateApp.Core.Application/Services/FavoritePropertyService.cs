using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class FavoritePropertyService(IFavoritePropertyRepository favoritePropertyRepository) : IFavoritePropertyService
    {
        private readonly IFavoritePropertyRepository _favoritePropertyRepository = favoritePropertyRepository;

        public async Task<List<int>> GetFavoritePropertyIdsByUserIdAsync(string userId)
        {
            var favorites = await _favoritePropertyRepository.GetByUserIdAsync(userId);
            return favorites.Select(f => f.PropertyId).ToList();
        }

        public async Task ToggleFavoriteAsync(string userId, int propertyId)
        {
            var favorite = await _favoritePropertyRepository.GetByUserIdAndPropertyIdAsync(userId, propertyId);

            if (favorite != null)
            {
                await _favoritePropertyRepository.RemoveAsync(favorite);
            }
            else
            {
                await _favoritePropertyRepository.AddAsync(new FavoriteProperty
                {
                    UserId = userId,
                    PropertyId = propertyId
                });
            }
        }
    }
}
