using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType
{
    public class CreatePropertyTypeCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreatePropertyTypeCommandHandler(IPropertyTypeRepository repository, IMapper mapper) : IRequestHandler<CreatePropertyTypeCommand, bool>
    {
        private readonly IPropertyTypeRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> Handle(CreatePropertyTypeCommand request, CancellationToken cancellationToken)
        {
            var propertyType = _mapper.Map<PropertyType>(request);
            await _repository.AddAsync(propertyType);
            return true;
        }
    }
}
