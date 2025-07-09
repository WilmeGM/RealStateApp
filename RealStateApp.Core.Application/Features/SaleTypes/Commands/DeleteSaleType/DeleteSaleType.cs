using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.DeleteSaleType
{
    public class DeleteSaleTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository) : IRequestHandler<DeleteSaleTypeCommand, bool>
    {
        private readonly ISaleTypeRepository _saleTypeRepository = saleTypeRepository;

        public async Task<bool> Handle(DeleteSaleTypeCommand request, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(request.Id)
                           ?? throw new Exception("SaleType not found");

            await _saleTypeRepository.RemoveAsync(saleType);
            return true;
        }
    }
}
