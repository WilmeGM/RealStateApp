using RealStateApp.Core.Application.ViewModels.Offer;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IOfferService
    {
        Task<List<OfferViewModel>> GetOffersByPropertyAndUserAsync(int propertyId, string userId);
        Task<bool> HasApprovedOrPendingOfferAsync(int propertyId, string userId);
        Task CreateOfferAsync(CreateOfferViewModel offer);
        Task<List<string>> GetClientsWithOffersByPropertyAsync(int propertyId);
        Task AcceptOfferAsync(int offerId, int propertyId);
        Task RejectOfferAsync(int offerId);
        Task<string> GetClientIdByOfferAsync(int offerId);
    }
}
