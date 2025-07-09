namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IFavoritePropertyService
    {
        Task<List<int>> GetFavoritePropertyIdsByUserIdAsync(string userId);
        Task ToggleFavoriteAsync(string userId, int propertyId);
    }
}
