using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeById
{
    public class GetPropertyTypeByIdQuery : IRequest<PropertyTypeDto>
    {
        public int Id { get; set; }
    }

    public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, PropertyTypeDto>
    {
        private readonly IPropertyTypeRepository _repository;
        private readonly IMapper _mapper;

        public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PropertyTypeDto> Handle(GetPropertyTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var propertyType = await _repository.GetByIdAsync(request.Id)
                             ?? throw new Exception("PropertyType not found");

            return _mapper.Map<PropertyTypeDto>(propertyType);
        }
    }
}
