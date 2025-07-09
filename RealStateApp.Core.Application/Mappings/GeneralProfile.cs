using AutoMapper;
using RealStateApp.Core.Application.Dtos;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Account;
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region Auth
            CreateMap<LoginRequest, LoginViewModel>()
                .ReverseMap();
       
            CreateMap<RegisterRequest, RegisterViewModel>()
                .ForMember(src => src.Photo, opt => opt.Ignore())
                .ForMember(src => src.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(src => src.PhotoUrl, opt => opt.Ignore());
            #endregion

            #region Admin
            CreateMap<CreateAdminUserRequest, SaveAdminViewModel>()
                .ReverseMap();
            ///////////////////////////////////////////////////
            CreateMap<UserAdminResponse, AdminViewModel>()
                .ReverseMap();
            ///////////////////////////////////////////////////
            CreateMap<UserAdminResponse, UpdateAdminViewModel>()
                .ForMember(x => x.Password, opt => opt.Ignore())
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.IsActive, opt => opt.Ignore());
            ///////////////////////////////////////////////////
            CreateMap<UpdateAdminUserRequest, UpdateAdminViewModel>()
                .ReverseMap();
            ///////////////////////////////////////////////////
            CreateMap<UpdateAdminUserResponse, UpdateAdminViewModel>()
                .ReverseMap();
            #endregion

            #region Developer
            ///////////////////Developers///////////////////////////////
            CreateMap<CreateDevelopersUserRequest, SaveDevelopersViewModel>()
                .ReverseMap();
            ///////////////////Developers///////////////////////////////
            CreateMap<UserDevelopersResponse, DevelopersViewModel>()
                .ReverseMap();
            ///////////////////////////////////////////////////
            CreateMap<UserDevelopersResponse, UpdateDevelopersViewModel>()
                .ForMember(x => x.Password, opt => opt.Ignore())
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.IsActive, opt => opt.Ignore());
            ///////////////////////////////////////////////////
            CreateMap<UpdateDevelopersUserRequest, UpdateDevelopersViewModel>()
                .ReverseMap();
            ///////////////////////////////////////////////////
            CreateMap<UpdateDevelopersUserResponse, UpdateDevelopersViewModel>()
                .ReverseMap();
            #endregion

            #region Offer
            CreateMap<Offer, OfferViewModel>()
                .ReverseMap()
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.PropertyId, opt => opt.Ignore())
                .ForMember(x => x.Property, opt => opt.Ignore());

            CreateMap<Offer, CreateOfferViewModel>()
                .ReverseMap();
            #endregion
        }
    }
}
