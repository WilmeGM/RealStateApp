using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Features.Improvemets.Commands.CreateImprovement
{
    public class CreateImprovementCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateImprovementCommandHandler(
        IImprovementRepository improvementRepository,
        IMapper mapper
            ) : IRequestHandler<CreateImprovementCommand, bool>
    {
        private readonly IImprovementRepository _improvementRepository = improvementRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> Handle
            (
            CreateImprovementCommand request,
            CancellationToken cancellationToken
            )
        {
            var improvement = _mapper.Map<Improvement>(request);
            await _improvementRepository.AddAsync(improvement);
            return true;
        }
    }
}
