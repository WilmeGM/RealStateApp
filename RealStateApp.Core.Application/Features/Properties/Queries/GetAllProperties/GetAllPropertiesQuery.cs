using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<List<PropertyDto>>
    {

    }

    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, List<PropertyDto>>
    {
        private readonly IPropertyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPropertiesQueryHandler(IPropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = await _repository.GetAllAsync();
            return _mapper.Map<List<PropertyDto>>(properties);
        }
    }
}
