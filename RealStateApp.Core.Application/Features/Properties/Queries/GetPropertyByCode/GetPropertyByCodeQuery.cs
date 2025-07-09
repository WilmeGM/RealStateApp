using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Interfaces.Repositories;
using AutoMapper;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetPropertyByCode
{
    public class GetPropertyByCodeQuery : IRequest<PropertyDto>
    {
        public string Code { get; set; }
    }

    public class GetPropertyByCodeQueryHandler : IRequestHandler<GetPropertyByCodeQuery, PropertyDto>
    {
        private readonly IPropertyRepository _repository;
        private readonly IMapper _mapper;

        public GetPropertyByCodeQueryHandler(IPropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(GetPropertyByCodeQuery request, CancellationToken cancellationToken)
        {
            var property = (await _repository.GetAllAsync())
                           .FirstOrDefault(p => p.UniqueCode == request.Code);

            if (property == null) throw new Exception("Property not found");

            return _mapper.Map<PropertyDto>(property);
        }
    }
}
