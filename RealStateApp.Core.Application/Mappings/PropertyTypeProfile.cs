using AutoMapper;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings
{
    public class PropertyTypeProfile : Profile
    {
        public PropertyTypeProfile()
        {
            CreateMap<PropertyType, PropertyTypeViewModel>()
               .ForMember(x => x.PropertyCount, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(x => x.CreatedAt, opt => opt.Ignore())
               .ForMember(x => x.UpdatedAt, opt => opt.Ignore());

            CreateMap<PropertyType, SavePropertyTypeViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore());

            CreateMap<PropertyType, UpdatePropertyTypeViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore());

            CreateMap<PropertyType, PropertyTypeDto>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.Properties, opt => opt.Ignore());

            CreateMap<PropertyType, CreatePropertyTypeCommand>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.Properties, opt => opt.Ignore());

            CreateMap<PropertyType, UpdatePropertyTypeCommand>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.Properties, opt => opt.Ignore());
        }
    }
}
