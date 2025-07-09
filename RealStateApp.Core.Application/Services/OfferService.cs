using AutoMapper;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Services
{
    public class OfferService(IOfferRepository offerRepository,
        IMapper mapper,
        IPropertyRepository propertyRepository) : IOfferService
    {
        private readonly IOfferRepository _offerRepository = offerRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IPropertyRepository _propertyRepository = propertyRepository;

        public async Task<List<OfferViewModel>> GetOffersByPropertyAndUserAsync(int propertyId, string userId)
        {
            var offers = await _offerRepository.GetOffersByPropertyAndUserAsync(propertyId, userId);
            return _mapper.Map<List<OfferViewModel>>(offers);
        }

        public async Task<bool> HasApprovedOrPendingOfferAsync(int propertyId, string userId)
        {
            var offers = await _offerRepository.GetOffersByPropertyAsync(propertyId);
            return offers.Any(o => o.OfferStatus == OfferStatus.Accepted.ToString() ||
            (o.OfferStatus == OfferStatus.Pending.ToString() && o.UserId == userId));
        }

        public async Task CreateOfferAsync(CreateOfferViewModel offer)
        {
            var entity = _mapper.Map<Offer>(offer);
            entity.OfferStatus = OfferStatus.Pending.ToString();

            await _offerRepository.AddAsync(entity);
        }

        public async Task<List<string>> GetClientsWithOffersByPropertyAsync(int propertyId)
        {
            var offers = await _offerRepository.GetOffersByPropertyAsync(propertyId);

            return offers
                .Select(o => o.UserId)
                .Distinct()
                .ToList();
        }

        public async Task AcceptOfferAsync(int offerId, int propertyId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);

            if (offer == null || offer.OfferStatus != OfferStatus.Pending.ToString())
                throw new InvalidOperationException("Offer not valid for acceptance.");

            // Cambiar el estado de la oferta aceptada
            offer.OfferStatus = OfferStatus.Accepted.ToString();
            await _offerRepository.UpdateAsync(offer, offerId);

            // Rechazar otras ofertas pendientes para esta propiedad
            var pendingOffers = await _offerRepository.GetOffersByPropertyAsync(propertyId);
            foreach (var pendingOffer in pendingOffers.Where(o => o.Id != offerId && o.OfferStatus == OfferStatus.Pending.ToString()))
            {
                pendingOffer.OfferStatus = OfferStatus.Rejected.ToString();
                await _offerRepository.UpdateAsync(pendingOffer, offerId);
            }

            // Marcar la propiedad como vendida
            var property = await _propertyRepository.GetByIdAsync(propertyId);
            if (property != null)
            {
                property.PropertyStatus = PropertyStatus.Sold.ToString();
                await _propertyRepository.UpdateAsync(property, propertyId);
            }
        }

        public async Task RejectOfferAsync(int offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);

            if (offer == null || offer.OfferStatus != OfferStatus.Pending.ToString())
                throw new InvalidOperationException("Offer not valid for rejection.");

            offer.OfferStatus = OfferStatus.Rejected.ToString();
            await _offerRepository.UpdateAsync(offer, offerId);
        }

        public async Task<string> GetClientIdByOfferAsync(int offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            return offer?.UserId ?? string.Empty;
        }
    }
}
