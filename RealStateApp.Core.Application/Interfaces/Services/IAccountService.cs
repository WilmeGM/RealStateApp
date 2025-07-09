using RealStateApp.Core.Application.Dtos;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.User;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest, string origin);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task SignOutAsync();
        Task<CreateAdminUserResponse> CreateAdminUserAsync(CreateAdminUserRequest req);
        Task<List<UserAdminResponse>> GetAllAdminsAsync();
        Task<UserAdminResponse> GetAdminByIdAsync(string id);
        Task<UpdateAdminUserResponse> UpdateAdminUserAsync(UpdateAdminUserRequest req);
        Task<CreateDevelopersUserResponse> CreateDevelopersUserAsync(CreateDevelopersUserRequest req);
        Task<UpdateDevelopersUserResponse> UpdateDevelopersUserAsync(UpdateDevelopersUserRequest req);
        Task<List<UserDevelopersResponse>> GetAllDevelopersAsync();
        Task<UserDevelopersResponse> GetDevelopersByIdAsync(string id);
        Task<bool> UpdateDevelopersStatus(string id);
        Task<bool> UpdateAdminStatus(string id);
        Task<UpdateAgentResponse> GetAgentUpdateResponseByIdAsync(string id);
        Task UpdateAgentAsync(UpdateAgentRequest request);
        Task<List<UserResponse>> GetAllActiveUsersByRoleAsync(string role);
        Task<List<UserResponse>> GetAllAgentsAsync();
        Task<UserResponse> GetUserByIdAsync(string id);
        Task<int> GetAllActiveUsersCountByRoleAsync(string role);
        Task<int> GetAllInactiveUsersCountByRoleAsync(string role);
        Task<UserResponse> GetClientByIdAsync(string clientId);
        Task<AgentDto> GetAgentByIdAsync(string agentId);
        Task<List<AgentDto>> GetAllAgentsDtosAsync();
    }
}
