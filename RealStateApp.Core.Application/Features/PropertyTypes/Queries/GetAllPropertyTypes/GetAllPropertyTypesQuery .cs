using MediatR;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypes
{
    public class GetAllPropertyTypesQuery : IRequest<IEnumerable<PropertyTypeDto>>
    {
    }

    public class GetAllPropertyTypesQueryHandler : IRequestHandler<GetAllPropertyTypesQuery, IEnumerable<PropertyTypeDto>>
    {
        private readonly IPropertyTypeRepository _repository;

        public GetAllPropertyTypesQueryHandler(IPropertyTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PropertyTypeDto>> Handle(GetAllPropertyTypesQuery request, CancellationToken cancellationToken)
        {
            var propertyTypes = await _repository.GetAllAsync();

            return propertyTypes.Select(propertyType => new PropertyTypeDto
            {
                Id = propertyType.Id,
                Name = propertyType.Name,
                Description = propertyType.Description
            });
        }
    }
}
