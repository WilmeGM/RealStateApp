using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.Improvemets.Queries.GetImprovementById
{
    public class GetImprovementByIdQuery : IRequest<ImprovementDto>
    {
        public int Id { get; set; }
    }

    public class GetImprovementByIdQueryHandler
        (IImprovementRepository improvementRepository, IMapper mapper)
        : IRequestHandler<GetImprovementByIdQuery, ImprovementDto>
    {
        private readonly IImprovementRepository _improvementRepository = improvementRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ImprovementDto> Handle(GetImprovementByIdQuery request, CancellationToken cancellationToken)
        {
            var improvements = await _improvementRepository.GetAllAsync();
            var improvement = improvements.FirstOrDefault(i => i .Id == request.Id) ??
                throw new Exception("Improvement not found");
            var response = _mapper.Map<ImprovementDto>(improvement);
            return response;
        }
    }
}
