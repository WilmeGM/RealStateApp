using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Property
{
    public class SavePropertyViewModel
    {
        public int Id { get; set; }

        public string? UniqueCode { get; set; }

        [Required(ErrorMessage = "The property type is required")]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "The sale type is required")]
        public int SaleTypeId { get; set; }

        public string? UserId { get; set; }

        [Required(ErrorMessage = "The price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The size in meters is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Size must be greater than 0.")]
        public float SizeInMeters { get; set; }

        [Required(ErrorMessage = "The room count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Room count must be at least 1.")]
        public int RoomCount { get; set; }

        [Required(ErrorMessage = "The bathroom count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Bathroom count must be at least 1.")]
        public int BathroomCount { get; set; }

        [Required(ErrorMessage = "At least one improvement is required.")]
        public List<int> ImprovementIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "At least one image is required.")]
        public List<IFormFile> Images { get; set; } = [];
    }
}
