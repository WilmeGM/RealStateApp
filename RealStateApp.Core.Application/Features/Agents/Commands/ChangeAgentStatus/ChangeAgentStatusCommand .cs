using MediatR;
using RealStateApp.Core.Application.Interfaces.Services;

namespace RealStateApp.Core.Application.Features.Agents.Commands.ChangeAgentStatus
{
    public class ChangeAgentStatusCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
    }

    public class ChangeAgentStatusCommandHandler : IRequestHandler<ChangeAgentStatusCommand, bool>
    {
        private readonly IAccountService _accountService;

        public ChangeAgentStatusCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<bool> Handle(ChangeAgentStatusCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.UpdateAdminStatus(request.Id);
        }
    }
}
