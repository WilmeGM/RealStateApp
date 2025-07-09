using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Account;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<LoginResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(RegisterViewModel registerViewModel, string origin);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<CreateAdminUserResponse> CreateAdminUserAsync(SaveAdminViewModel vm);
        Task<List<AdminViewModel>> GetAllAdminsAsync();
        Task<UpdateAdminUserResponse> UpdateAdminUserAsync(UpdateAdminViewModel vm);
        Task<AdminViewModel> GetAdminByIdAsync(string id);
        Task<UpdateAdminViewModel> GetUpdateAdminByIdAsync(string id);
        Task<CreateDevelopersUserResponse> CreateDevelopersUserAsync(SaveDevelopersViewModel vm);
        Task<List<DevelopersViewModel>> GetAllDevelopersAsync();
        Task<DevelopersViewModel> GetDevelopersByIdAsync(string id);
        Task<UpdateDevelopersViewModel> GetUpdateDevelopersByIdAsync(string id);
        Task<UpdateDevelopersUserResponse> UpdateDevelopersUserAsync(UpdateDevelopersViewModel vm);
        Task SignOutAsync();
    }
}
