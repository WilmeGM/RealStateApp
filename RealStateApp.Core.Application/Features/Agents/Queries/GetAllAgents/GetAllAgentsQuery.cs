using MediatR;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Interfaces.Services;

namespace RealStateApp.Core.Application.Features.Agents.Queries.GetAllAgents
{
    public class GetAllAgentsQuery : IRequest<List<AgentDto>>
    {
    }

    public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, List<AgentDto>>
    {
        private readonly IAccountService _accountService;

        public GetAllAgentsQueryHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<List<AgentDto>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
        {
            var agents = await _accountService.GetAllAgentsDtosAsync();
            return agents;
        }
    }
}
