using AutoMapper;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Agent;

namespace RealStateApp.Core.Application.Services
{
    public class AgentService(IAccountService accountService,
        IMapper mapper,
        IPropertyRepository propertyRepository) : IAgentService
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;
        private readonly IPropertyRepository _propertyRepository = propertyRepository;

        public async Task<UpdateAgentViewModel> GetAgentUpdateViewModelByIdAsync(string id)
        {
            var agent = await _accountService.GetAgentUpdateResponseByIdAsync(id);
            var uvm = _mapper.Map<UpdateAgentViewModel>(agent);
            return uvm;
        }

        public async Task UpdateAgentAsync(UpdateAgentViewModel updateAgentViewModel)
        {
            var request = _mapper.Map<UpdateAgentRequest>(updateAgentViewModel);
            await _accountService.UpdateAgentAsync(request);
        }

        public async Task<List<AgentViewModel>> GetAllActiveAgentsAsync()
        {
            var agents = await _accountService.GetAllActiveUsersByRoleAsync(Roles.Agent.ToString());
            return _mapper.Map<List<AgentViewModel>>(agents);
        }

        public async Task<AgentViewModel> GetAgentByIdAsync(string id)
        {
            var agent = await _accountService.GetUserByIdAsync(id);
            return _mapper.Map<AgentViewModel>(agent);
        }

        public async Task<List<AgentViewModel>> GetAllAgentsAsync()
        {
            var agents = await _accountService.GetAllAgentsAsync();
            return _mapper.Map<List<AgentViewModel>>(agents);
        }

        public async Task<UserResponse> GetClientByIdAsync(string clientId)
            => await _accountService.GetClientByIdAsync(clientId);
    }
}
