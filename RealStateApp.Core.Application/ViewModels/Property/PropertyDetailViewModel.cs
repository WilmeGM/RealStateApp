using RealStateApp.Core.Application.ViewModels.ChatMessage;
using RealStateApp.Core.Application.ViewModels.Offer;

namespace RealStateApp.Core.Application.ViewModels.Property
{
    public class PropertyDetailViewModel
    {
        public PropertyViewModel Property { get; set; }
        public List<ChatMessageViewModel> Messages { get; set; }
        public List<OfferViewModel> Offers { get; set; }
        public bool CanMakeOffer { get; set; }
    }
}
