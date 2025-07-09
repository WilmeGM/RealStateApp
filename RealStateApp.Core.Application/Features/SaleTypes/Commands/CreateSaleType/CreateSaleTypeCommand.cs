using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.CreateSaleType
{
    public class CreateSaleTypeCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper) : IRequestHandler<CreateSaleTypeCommand, bool>
    {
        private readonly ISaleTypeRepository _saleTypeRepository = saleTypeRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> Handle(CreateSaleTypeCommand request, CancellationToken cancellationToken)
        {
            var saleType = _mapper.Map<SaleType>(request);
            await _saleTypeRepository.AddAsync(saleType);
            return true;
        }
    }
}
