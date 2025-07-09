using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyType
{
    public class DeletePropertyTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeletePropertyTypeCommandHandler : IRequestHandler<DeletePropertyTypeCommand, bool>
    {
        private readonly IPropertyTypeRepository _repository;

        public DeletePropertyTypeCommandHandler(IPropertyTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeletePropertyTypeCommand request, CancellationToken cancellationToken)
        {
            var propertyType = await _repository.GetByIdAsync(request.Id)
                             ?? throw new Exception("PropertyType not found");

            await _repository.RemoveAsync(propertyType);
            return true;
        }
    }
}
