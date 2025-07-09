using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.ViewModels.Property
{
    public class PropertyViewModel
    {
        public int Id { get; set; }
        public string UniqueCode { get; set; }
        public int PropertyTypeId { get; set; }
        public string PropertyType { get; set; }
        public int SaleTypeId { get; set; }
        public string SaleType { get; set; }
        public string Description { get; set; }
        public List<string> ImageUrls { get; set; } = [];
        public decimal Price { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public float SizeInMeters { get; set; }
        public string UserId { get; set; }
        public string PropertyStatus { get; set; }
        public ICollection<ImprovementViewModel> Improvements { get; set; }
        public ICollection<OfferViewModel> Offers { get; set; }
        public string StatusLabel { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
