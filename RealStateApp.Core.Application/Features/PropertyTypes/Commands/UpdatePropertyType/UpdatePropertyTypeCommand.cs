using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType
{
    public class UpdatePropertyTypeCommand : IRequest<PropertyTypeDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdatePropertyTypeCommandHandler : IRequestHandler<UpdatePropertyTypeCommand, PropertyTypeDto>
    {
        private readonly IPropertyTypeRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePropertyTypeCommandHandler(IPropertyTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PropertyTypeDto> Handle(UpdatePropertyTypeCommand request, CancellationToken cancellationToken)
        {
            var propertyType = await _repository.GetByIdAsync(request.Id)
                             ?? throw new Exception("PropertyType not found");

            propertyType.Name = request.Name;
            propertyType.Description = request.Description;

            await _repository.UpdateAsync(propertyType, propertyType.Id);

            return _mapper.Map<PropertyTypeDto>(propertyType);
        }
    }
}
