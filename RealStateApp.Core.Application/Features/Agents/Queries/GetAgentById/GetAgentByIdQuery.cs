using MediatR;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Interfaces.Services;

namespace RealStateApp.Core.Application.Features.Agents.Queries.GetAgentById
{
    public class GetAgentByIdQuery : IRequest<AgentDto>
    {
        public string Id { get; set; }
    }

    public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, AgentDto>
    {
        private readonly IAccountService _accountService;

        public GetAgentByIdQueryHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<AgentDto> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
        {
            var agent = await _accountService.GetAgentByIdAsync(request.Id);

            if (agent == null)
                throw new Exception("Agent not found");

            return agent;
        }
    }
}
