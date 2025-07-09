using AutoMapper;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.CreateSaleType;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings
{
    public class SaleTypeProfile : Profile
    {
        public SaleTypeProfile() 
        {
            CreateMap<SaleType, SaleTypeViewModel>()
               .ForMember(x => x.PropertyCount, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(x => x.CreatedAt, opt => opt.Ignore())
               .ForMember(x => x.UpdatedAt, opt => opt.Ignore());

            CreateMap<SaleType, SaveSaleTypeViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore());

            CreateMap<SaleType, UpdateSaleTypeViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore());

            CreateMap<SaleType, SaleTypeDto>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.Properties, opt => opt.Ignore());

            CreateMap<SaleType, CreateSaleTypeCommand>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.Properties, opt => opt.Ignore());
        }
    }
}
