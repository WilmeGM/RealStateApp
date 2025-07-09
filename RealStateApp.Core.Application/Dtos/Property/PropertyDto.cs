using RealStateApp.Core.Application.Dtos.Improvement;

namespace RealStateApp.Core.Application.Dtos.Property
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string UniqueCode { get; set; }
        public int PropertyTypeId { get; set; }
        public int SaleTypeId { get; set; }
        public decimal Price { get; set; }
        public float SizeInMeters { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public ICollection<ImprovementDto> Improvements { get; set; }
        public string PropertyStatus { get; set; }

    }
}
