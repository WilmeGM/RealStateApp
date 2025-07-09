using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Core.Application.Features.Improvemets.Commands.DeleteImprovementById
{
    public class DeleteImprovementByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteImprovementByIdCommandHandler
        (IImprovementRepository improvementRepository)
        : IRequestHandler<DeleteImprovementByIdCommand, bool>
    {
        private readonly IImprovementRepository _improvementRepository = improvementRepository;

        public async Task<bool> Handle
            (
            DeleteImprovementByIdCommand command,
            CancellationToken cancellationToken
            )
        {
            var improvement = await _improvementRepository.GetByIdAsync(command.Id)
                ?? throw new Exception("Improvement not found");

            await _improvementRepository.RemoveAsync(improvement);
            return true;
        }
    }
}
