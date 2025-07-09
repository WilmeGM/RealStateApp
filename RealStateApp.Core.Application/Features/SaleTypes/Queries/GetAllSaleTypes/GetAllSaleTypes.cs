using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.SaleTypes.Queries.GetAllSaleTypes
{
    public class GetAllSaleTypesQuery : IRequest<IEnumerable<SaleTypeDto>>
    {
    }

    public class GetAllSaleTypesQueryHandler(ISaleTypeRepository saleTypeRepository) : IRequestHandler<GetAllSaleTypesQuery, IEnumerable<SaleTypeDto>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository = saleTypeRepository;

        public async Task<IEnumerable<SaleTypeDto>> Handle(GetAllSaleTypesQuery request, CancellationToken cancellationToken)
        {
            var saleTypes = await _saleTypeRepository.GetAllAsync();

            return saleTypes.Select(saleType => new SaleTypeDto
            {
                Id = saleType.Id,
                Name = saleType.Name,
                Description = saleType.Description
            });
        }
    }
}
