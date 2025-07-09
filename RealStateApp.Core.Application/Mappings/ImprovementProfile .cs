using AutoMapper;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Features.Improvemets.Commands.CreateImprovement;
using RealStateApp.Core.Application.Features.Improvemets.Commands.UpdateImprovement;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings
{
    public class ImprovementProfile : Profile
    {
        public ImprovementProfile()
        {
            CreateMap<Improvement, ImprovementViewModel>()
               .ReverseMap()
               .ForMember(x => x.CreatedAt, opt => opt.Ignore())
               .ForMember(x => x.UpdatedAt, opt => opt.Ignore());

            CreateMap<Improvement, SaveImprovementViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore());

            CreateMap<Improvement, UpdateImprovementViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore());

            CreateMap<Improvement, ImprovementDto>();

            CreateMap<CreateImprovementCommand, Improvement>();

            CreateMap<UpdateImprovementCommand, Improvement>();

            CreateMap<Improvement, ImprovementUpdateResponse>();
        }
    }
}
