using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Features.Improvemets.Commands.UpdateImprovement
{
    public class UpdateImprovementCommand : IRequest<ImprovementUpdateResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateImprovementCommandHandler
        (IImprovementRepository improvementRepository,
        IMapper mapper) : IRequestHandler<UpdateImprovementCommand, ImprovementUpdateResponse>
    {
        private readonly IImprovementRepository _improvementRepository = improvementRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ImprovementUpdateResponse> Handle
            (UpdateImprovementCommand request,
            CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(request.Id) ??
                throw new Exception("Improvement not found");

            improvement = _mapper.Map<Improvement>(request);
            await _improvementRepository.UpdateAsync(improvement, request.Id);
            var response = _mapper.Map<ImprovementUpdateResponse>(improvement);
            return response;
        }
    }
}
