using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.Agents.Queries.GetAgentProperties
{
    public class GetAgentPropertiesQuery : IRequest<List<PropertyDto>>
    {
        public string AgentId { get; set; }
    }

    public class GetAgentPropertiesQueryHandler : IRequestHandler<GetAgentPropertiesQuery, List<PropertyDto>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetAgentPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<List<PropertyDto>> Handle(GetAgentPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = await _propertyRepository.GetPropertiesByAgentIdAsync(request.AgentId);
            return _mapper.Map<List<PropertyDto>>(properties);
        }
    }
}
