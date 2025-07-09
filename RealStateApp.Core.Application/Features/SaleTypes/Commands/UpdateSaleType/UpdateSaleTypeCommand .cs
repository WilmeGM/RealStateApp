using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType
{
    public class UpdateSaleTypeCommand : IRequest<SaleTypeDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateSaleTypeCommandHandler
        (ISaleTypeRepository saleTypeRepository,
        IMapper mapper) : IRequestHandler<UpdateSaleTypeCommand, SaleTypeDto>
    {
        private readonly ISaleTypeRepository _saleTypeRepository = saleTypeRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<SaleTypeDto> Handle(UpdateSaleTypeCommand request, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(request.Id)
                           ?? throw new Exception("SaleType not found");

            saleType.Name = request.Name;
            saleType.Description = request.Description;

            await _saleTypeRepository.UpdateAsync(saleType, saleType.Id);

            return _mapper.Map<SaleTypeDto>(saleType);
        }
    }
}
