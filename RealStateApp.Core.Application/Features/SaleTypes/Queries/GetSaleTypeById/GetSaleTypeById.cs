using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById
{
    public class GetSaleTypeByIdQuery : IRequest<SaleTypeDto>
    {
        public int Id { get; set; }
    }

    public class GetSaleTypeByIdQueryHandler
        (ISaleTypeRepository saleTypeRepository,
        IMapper mapper) : IRequestHandler<GetSaleTypeByIdQuery, SaleTypeDto>
    {
        private readonly ISaleTypeRepository _saleTypeRepository = saleTypeRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<SaleTypeDto> Handle(GetSaleTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(request.Id)
                           ?? throw new Exception("SaleType not found");

            return _mapper.Map<SaleTypeDto>(saleType);
        }
    }
}
