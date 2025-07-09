using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.User;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IAccountServiceForWebApi
    {
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);
        Task<CreateAdminUserResponse> RegisterAdminUserAsync(CreateAdminUserRequest req);
        Task<CreateDevelopersUserResponse> RegisterDeveloperUserAsync(CreateDevelopersUserRequest req);
    }
}