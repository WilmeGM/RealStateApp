using AutoMapper;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyViewModel>()
                .ForMember(x => x.StatusLabel, opt => opt.Ignore())
                .ForMember(x => x.IsFavorite, opt => opt.Ignore())
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
                .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.SaleType.Name))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => new List<string> {
                    src.ImageUrl1, src.ImageUrl2, src.ImageUrl3, src.ImageUrl4
                }.Where(url => !string.IsNullOrEmpty(url)).ToList()))
                .ReverseMap();

            CreateMap<SavePropertyViewModel, Property>()
                .ForMember(dest => dest.Improvements, opt => opt.Ignore())
                .ForMember(dest => dest.PropertyStatus, opt => opt.MapFrom(src => PropertyStatus.Available.ToString()))
                .ReverseMap();

            CreateMap<Property, PropertyDto>()
                .ReverseMap()
                .ForMember(x => x.PropertyType, opt => opt.Ignore())
                .ForMember(x => x.SaleType, opt => opt.Ignore())
                .ForMember(x => x.ImageUrl1, opt => opt.Ignore())
                .ForMember(x => x.ImageUrl2, opt => opt.Ignore())
                .ForMember(x => x.ImageUrl3, opt => opt.Ignore())
                .ForMember(x => x.ImageUrl4, opt => opt.Ignore())
                .ForMember(x => x.Offers, opt => opt.Ignore())
                .ForMember(x => x.FavoriteProperties, opt => opt.Ignore())
                .ForMember(x => x.ChatMessages, opt => opt.Ignore());
        }
    }
}
