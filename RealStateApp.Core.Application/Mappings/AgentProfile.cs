using AutoMapper;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Agent;

namespace RealStateApp.Core.Application.Mappings
{
    public class AgentProfile : Profile
    {
        public AgentProfile()
        {
            CreateMap<UpdateAgentViewModel, UpdateAgentResponse>()
                .ReverseMap()
                .ForMember(x => x.Photo, opt => opt.Ignore());

            CreateMap<UpdateAgentViewModel, UpdateAgentRequest>()
                .ReverseMap()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.ErrorMessage, opt => opt.Ignore())
                .ForMember(x => x.Photo, opt => opt.Ignore());

            CreateMap<UserResponse, AgentViewModel>()
                .ReverseMap();
        }
    }
}
