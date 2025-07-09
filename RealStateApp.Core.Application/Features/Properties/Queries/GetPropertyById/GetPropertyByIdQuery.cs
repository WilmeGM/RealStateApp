using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Interfaces.Repositories;
using AutoMapper;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetPropertyById
{
    public class GetPropertyByIdQuery : IRequest<PropertyDto>
    {
        public int Id { get; set; }
    }

    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
    {
        private readonly IPropertyRepository _repository;
        private readonly IMapper _mapper;

        public GetPropertyByIdQueryHandler(IPropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _repository.GetByIdAsync(request.Id);
            if (property == null) throw new Exception("Property not found");

            return _mapper.Map<PropertyDto>(property);
        }
    }
}
