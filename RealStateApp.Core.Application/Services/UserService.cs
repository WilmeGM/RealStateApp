using AutoMapper;
using RealStateApp.Core.Application.Dtos;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Account;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.Services
{
    public class UserService(IAccountService accountService, IMapper mapper) : IUserService
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;

        public async Task<LoginResponse> LoginAsync(LoginViewModel vm)
        {
            LoginRequest loginRequest = _mapper.Map<LoginRequest>(vm);
            return await _accountService.LoginAsync(loginRequest);
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterViewModel registerViewModel, string origin)
        {
            string photoUrl = FileUploaderHelper.UploadImage(registerViewModel.Photo, Guid.NewGuid().ToString());
            var request = _mapper.Map<RegisterRequest>(registerViewModel);
            request.PhotoUrl = photoUrl;
            return await _accountService.RegisterAsync(request, origin); 
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token) => await _accountService.ConfirmAccountAsync(userId, token);

        public async Task<CreateAdminUserResponse> CreateAdminUserAsync(SaveAdminViewModel vm)
        {
            CreateAdminUserRequest createAdminUserRequest = _mapper.Map<CreateAdminUserRequest>(vm);
            return await _accountService.CreateAdminUserAsync(createAdminUserRequest);
        }
        public async Task<List<AdminViewModel>> GetAllAdminsAsync()
        {
            var adminList = await _accountService.GetAllAdminsAsync();
            var adminListViewModel = _mapper.Map<List<AdminViewModel>>(adminList);
            return adminListViewModel;
        }
        public async Task<AdminViewModel> GetAdminByIdAsync(string id)
        {
            var admin = await _accountService.GetAdminByIdAsync(id);
            var adminViewModel = _mapper.Map<AdminViewModel>(admin);
            return adminViewModel;
        }

        public async Task<UpdateAdminViewModel> GetUpdateAdminByIdAsync(string id)
        {
            var admin = await _accountService.GetAdminByIdAsync(id);
            var adminViewModel = _mapper.Map<UpdateAdminViewModel>(admin);
            return adminViewModel;
        }

        public async Task<UpdateAdminUserResponse> UpdateAdminUserAsync(UpdateAdminViewModel vm)
        {
            var request = _mapper.Map<UpdateAdminUserRequest>(vm);
            return await _accountService.UpdateAdminUserAsync(request);
        }

        //Metodos Developers//
        public async Task<CreateDevelopersUserResponse> CreateDevelopersUserAsync(SaveDevelopersViewModel vm)
        {
            CreateDevelopersUserRequest createDevelopersUserRequest = _mapper.Map<CreateDevelopersUserRequest>(vm);
            return await _accountService.CreateDevelopersUserAsync(createDevelopersUserRequest);
        }
        public async Task<List<DevelopersViewModel>> GetAllDevelopersAsync()
        {
            var developersList = await _accountService.GetAllDevelopersAsync();
            var developersListViewModel = _mapper.Map<List<DevelopersViewModel>>(developersList);
            return developersListViewModel;
        }
        public async Task<DevelopersViewModel> GetDevelopersByIdAsync(string id)
        {
            var developers = await _accountService.GetDevelopersByIdAsync(id);
            var developersViewModel = _mapper.Map<DevelopersViewModel>(developers);
            return developersViewModel;
        }

        public async Task<UpdateDevelopersViewModel> GetUpdateDevelopersByIdAsync(string id)
        {
            var developers = await _accountService.GetDevelopersByIdAsync(id);
            var developersViewModel = _mapper.Map<UpdateDevelopersViewModel>(developers);
            return developersViewModel;
        }

        public async Task<UpdateDevelopersUserResponse> UpdateDevelopersUserAsync(UpdateDevelopersViewModel vm)
        {
            var request = _mapper.Map<UpdateDevelopersUserRequest>(vm);
            return await _accountService.UpdateDevelopersUserAsync(request);
        }
        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }
    }
}
