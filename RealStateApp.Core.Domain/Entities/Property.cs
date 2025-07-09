using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities
{
    public class Property : AuditableBaseEntity
    {
        public string UniqueCode { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        public int SaleTypeId { get; set; }
        public SaleType SaleType { get; set; }
        public string Description { get; set; }
        public string? ImageUrl1 { get; set; }
        public string? ImageUrl2 { get; set; }
        public string? ImageUrl3 { get; set; }
        public string? ImageUrl4 { get; set; }
        public decimal Price { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public float SizeInMeters { get; set; }
        public string UserId { get; set; }
        public string PropertyStatus { get; set; }
        public ICollection<Improvement> Improvements { get; set; }
        public ICollection<Offer> Offers { get; set; }
        public ICollection<FavoriteProperty> FavoriteProperties { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
