using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Agent;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IAgentService
    {
        Task<UpdateAgentViewModel> GetAgentUpdateViewModelByIdAsync(string id);
        Task UpdateAgentAsync(UpdateAgentViewModel updateAgentViewModel);
        Task<List<AgentViewModel>> GetAllActiveAgentsAsync();
        Task<AgentViewModel> GetAgentByIdAsync(string id);
        Task<List<AgentViewModel>> GetAllAgentsAsync();
        Task<UserResponse> GetClientByIdAsync(string clientId);
    }
}
