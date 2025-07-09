using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.Improvemets.Queries.GetAllImprovements
{
    public class GetAllImprovementQuery : IRequest<IEnumerable<ImprovementDto>>
    {
    }

    public class GetAllImprovementQueryHandler
        (IImprovementRepository improvementRepository, IMapper mapper)
        : IRequestHandler<GetAllImprovementQuery, IEnumerable<ImprovementDto>>
    {
        private readonly IImprovementRepository _improvementRepository = improvementRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ImprovementDto>> Handle(GetAllImprovementQuery request, CancellationToken cancellationToken)
        {
            var improvements = await GetAllImprovementsAsync();
            return improvements;
        }

        private async Task<List<ImprovementDto>> GetAllImprovementsAsync()
        {
            var improvements = await _improvementRepository.GetAllAsync();

            return improvements.Select(improvement => new ImprovementDto
            {
                Name = improvement.Name,
                Description = improvement.Description,
                Id = improvement.Id,
            }).ToList();
        }
    }
}
