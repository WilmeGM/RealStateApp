using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Infrastructure.Identity.Entities;
using System.Text;
using RealStateApp.Core.Application.Dtos.Email;
using RealStateApp.Core.Application.Interfaces.Repositories;

namespace RealStateApp.Infrastructure.Identity.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService,
        IPropertyRepository propertyRepository,
        IPropertyService propertyService) : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IEmailService _emailService = emailService;
        private readonly IPropertyRepository _propertyRepository = propertyRepository;
        private readonly IPropertyService _propertyService = propertyService;

        #region Auth
        public async Task<LoginResponse> LoginAsync(LoginRequest req)
        {
            var res = new LoginResponse();

            var user = await _userManager.FindByEmailAsync(req.Email);

            if (user == null || !user.IsActive)
            {
                res.HasError = true;
                res.ErrorMessage = user == null ? "Incorrect credentials" : "Inactive user";
                return res;
            }

            if(await _userManager.IsInRoleAsync(user, Roles.Client.ToString()))
            {
                if (!user.EmailConfirmed)
                {
                    res.HasError = true;
                    res.ErrorMessage = "You have not confirmed your email";
                    return res;
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, req.Password, true, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                res.HasError = true;
                res.ErrorMessage = "Incorrect credentials";
                return res;
            }

            res.Id = user.Id;
            res.UserName = user.UserName;
            res.Email = user.Email;
            res.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            res.IsActive = user.IsActive;
            return res;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest, string origin)
        {
            var response = new RegisterResponse();
            response.HasError = false;
            response.ErrorMessage = null;

            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);

            if (existingUser is not null)
            {
                response.HasError = true;
                response.ErrorMessage = "Email is already taken";
                return response;
            }

            existingUser = await _userManager.FindByNameAsync(registerRequest.UserName);

            if (existingUser is not null)
            {
                response.HasError = true;
                response.ErrorMessage = "Username is already taken";
                return response;
            }

            var newUser = new ApplicationUser
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PhoneNumber = registerRequest.PhoneNumber,
                PhotoUrl = registerRequest.PhotoUrl,
                IsActive = false
            };

            var createResult = await _userManager.CreateAsync(newUser, registerRequest.Password);

            if (!createResult.Succeeded)
            {
                response.HasError = true;
                response.ErrorMessage = createResult.Errors.First().ToString();
                return response;
            }

            var userCreated = await _userManager.FindByEmailAsync(newUser.Email);

            response.Id = userCreated.Id;

            _ = registerRequest.Role == Roles.Agent.ToString() ?
                await _userManager.AddToRoleAsync(newUser, Roles.Agent.ToString()) :
                await _userManager.AddToRoleAsync(newUser, Roles.Client.ToString());

            var urlConfirmation = await GenerateEmailVerificationUri(newUser, origin);

            if (registerRequest.Role == Roles.Client.ToString())
            {
                try
                {
                    await _emailService.SendAsync(new EmailRequest
                    {
                        To = newUser.Email,
                        Subject = "Active your account",
                        Body = $"Active your account click on the next link: {urlConfirmation}"
                    });

                    return response;
                }
                catch (Exception ex)
                {
                    response.HasError = true;
                    response.ErrorMessage = $"User created ok, but an error ocurred sending the email: \n{ex.ToString()}";
                    return response;
                }
            }

            return response;
        }

        private async Task<string> GenerateEmailVerificationUri(ApplicationUser user, string origin)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "Account/ConfirmEmail";
            var uri = new Uri($"{origin}/{route}?userId={user.Id}&token={encodedToken}");
            return uri.ToString();
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return "User not found.";

            user.IsActive = true;
            await _userManager.UpdateAsync(user);

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded) return result.Errors.First().ToString();

            return "Account activated succesfully";
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        #endregion

        #region Admin
        public async Task<CreateAdminUserResponse> CreateAdminUserAsync(CreateAdminUserRequest req)
        {
            CreateAdminUserResponse res = new();

            var user = await _userManager.FindByNameAsync(req.UserName);
            if (user is not null)
            {
                res.HasError = true;
                res.ErrorMessage = $"that user name is taken";
                return res;
            }

            var email = await _userManager.FindByEmailAsync(req.Email);
            if (email is not null)
            {
                res.HasError = true;
                res.ErrorMessage = $"that email is taken";
                return res;
            }


            var userToCreate = new ApplicationUser()
            {
                FirstName = req.FirstName,
                UserName = req.UserName,
                LastName = req.LastName,
                IdCard = req.IdCard,
                Email = req.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(userToCreate, req.Password);
            if (!result.Succeeded)
            {
                res.HasError = true;
                res.ErrorMessage = $"An error ocurred trying to create the user";
                return res;

            }
            await _userManager.AddToRoleAsync(userToCreate, Roles.Admin.ToString());
            return res;
        }
        public async Task<UpdateAdminUserResponse> UpdateAdminUserAsync(UpdateAdminUserRequest req)
        {
            UpdateAdminUserResponse res = new();

            var user = await _userManager.FindByIdAsync(req.Id);
            if (user is null)
            {
                res.HasError = true;
                res.ErrorMessage = $"user not found";
                return res;
            }

            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.IdCard = req.IdCard;


            if (user.UserName != req.UserName)
            {
                var userWithSameUserName = await _userManager.FindByNameAsync(req.UserName);
                if (userWithSameUserName is not null)
                {
                    res.HasError = true;
                    res.ErrorMessage = $"That Username is taken";
                    return res;
                }
                user.UserName = req.UserName;

            }
            if (user.Email != req.Email)
            {
                var userWithSameEmail = await _userManager.FindByEmailAsync(req.Email);
                if (userWithSameEmail is not null)
                {
                    res.HasError = true;
                    res.ErrorMessage = $"That Email is taken";
                    return res;
                }
                user.Email = req.Email;
            }
            if (req.Password is not null)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, req.Password);

            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                res.HasError = true;
                res.ErrorMessage = $"An error ocurred trying to create the user";
                return res;
            }
            return res;
        }
        public async Task<List<UserAdminResponse>> GetAllAdminsAsync()
        {
            var userlist = await _userManager.Users.ToListAsync();

            var adminlist = new List<UserAdminResponse>();
            foreach (var user in userlist)
            {
                if (await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
                {
                    adminlist.Add(new UserAdminResponse
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IdCard = user.IdCard,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        IsActive = user.IsActive
                    });
                }

            }
            return adminlist;
        }
        public async Task<UserAdminResponse> GetAdminByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userResponse = new UserAdminResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IdCard = user.IdCard,
                UserName = user.UserName,
                IsActive = user.IsActive

            };

            return userResponse;
        }
        public async Task<bool> UpdateAdminStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);

            return true;
        }
        #endregion

        #region Agent
        public async Task<UpdateAgentResponse> GetAgentUpdateResponseByIdAsync(string id)
        {
            var agent = await _userManager.FindByIdAsync(id);
            var agentResponse = new UpdateAgentResponse
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                PhoneNumber = agent.PhoneNumber,
                PhotoUrl = agent.PhotoUrl,
            };
            return agentResponse;
        }

        public async Task UpdateAgentAsync(UpdateAgentRequest request)
        {
            var agent = await _userManager.FindByIdAsync(request.Id);

            if (agent != null)
            {
                agent.FirstName = request.FirstName;
                agent.LastName = request.LastName;
                agent.PhoneNumber = request.PhoneNumber;
                agent.PhotoUrl = request.PhotoUrl;

                await _userManager.UpdateAsync(agent);
            }
            else
            {
                throw new Exception("Agent not found");
            }
        }

        public async Task<List<UserResponse>> GetAllAgentsAsync()
        {
            var agents = await _userManager.GetUsersInRoleAsync(Roles.Agent.ToString());

            var userResponses = new List<UserResponse>();

            foreach (var agent in agents)
            {
                var propertyCount = await _propertyRepository.GetPropertyCountByAgentId(agent.Id);

                userResponses.Add(new UserResponse
                {
                    Id = agent.Id,
                    FullName = $"{agent.FirstName} {agent.LastName}",
                    Email = agent.Email,
                    PhotoUrl = agent.PhotoUrl,
                    PhoneNumber = agent.PhoneNumber,
                    IsActive = agent.IsActive,
                    PropertyCount = propertyCount,
                });
            }

            return userResponses;
        }

        public async Task<List<AgentDto>> GetAllAgentsDtosAsync()
        {
            var agents = await _userManager.GetUsersInRoleAsync(Roles.Agent.ToString());

            var userResponses = new List<AgentDto>();

            foreach (var agent in agents)
            {
                var propertyCount = await _propertyRepository.GetPropertyCountByAgentId(agent.Id);

                userResponses.Add(new AgentDto
                {
                    Id = agent.Id,
                    FirstName = agent.FirstName,
                    LastName = agent.LastName,
                    Email = agent.Email,
                    Phone = agent.PhoneNumber,
                    PropertyCount = propertyCount,
                });
            }

            return userResponses;
        }


        public async Task<AgentDto> GetAgentByIdAsync(string agentId)
        {
            var agent = await _userManager.FindByIdAsync(agentId);
            if (agent == null) return null;

            return new AgentDto
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                PropertyCount = await _propertyRepository.GetPropertyCountByAgentId(agent.Id),
                Phone = agent.PhoneNumber,
                Email = agent.Email
            };
        }
        #endregion

        #region Generics
        public async Task<List<UserResponse>> GetAllActiveUsersByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users.Where(u => u.IsActive)
                        .Select(u => new UserResponse
                        {
                            Id = u.Id,
                            FullName = $"{u.FirstName} {u.LastName}",
                            PhotoUrl = u.PhotoUrl
                        }).ToList();
        }

        public async Task<UserResponse> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return new UserResponse
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                PhotoUrl = user.PhotoUrl,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }
        #endregion

        public async Task<int> GetAllActiveUsersCountByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users.Where(u => u.IsActive == true).Count();
        }

        public async Task<int> GetAllInactiveUsersCountByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users.Where(u => u.IsActive == false).Count();
        }

        #region Developers
        public async Task<CreateDevelopersUserResponse> CreateDevelopersUserAsync(CreateDevelopersUserRequest req)
        {
            CreateDevelopersUserResponse res = new();

            var user = await _userManager.FindByNameAsync(req.UserName);
            if (user is not null)
            {
                res.HasError = true;
                res.ErrorMessage = $"that user name is taken";
                return res;
            }

            var email = await _userManager.FindByEmailAsync(req.Email);
            if (email is not null)
            {
                res.HasError = true;
                res.ErrorMessage = $"that email is taken";
                return res;
            }

            var userToCreate = new ApplicationUser()
            {
                FirstName = req.FirstName,
                UserName = req.UserName,
                LastName = req.LastName,
                IdCard = req.IdCard,
                Email = req.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(userToCreate, req.Password);
            if (!result.Succeeded)
            {
                res.HasError = true;
                res.ErrorMessage = $"An error ocurred trying to create the user";
                return res;

            }
            await _userManager.AddToRoleAsync(userToCreate, Roles.Developer.ToString());
            return res;
        }
        public async Task<UpdateDevelopersUserResponse> UpdateDevelopersUserAsync(UpdateDevelopersUserRequest req)
        {
            UpdateDevelopersUserResponse res = new();

            var user = await _userManager.FindByIdAsync(req.Id);
            if (user is null)
            {
                res.HasError = true;
                res.ErrorMessage = $"user not found";
                return res;
            }

            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.IdCard = req.IdCard;


            if (user.UserName != req.UserName)
            {
                var userWithSameUserName = await _userManager.FindByNameAsync(req.UserName);
                if (userWithSameUserName is not null)
                {
                    res.HasError = true;
                    res.ErrorMessage = $"That Username is taken";
                    return res;
                }
                user.UserName = req.UserName;

            }
            if (user.Email != req.Email)
            {
                var userWithSameEmail = await _userManager.FindByEmailAsync(req.Email);
                if (userWithSameEmail is not null)
                {
                    res.HasError = true;
                    res.ErrorMessage = $"That Email is taken";
                    return res;
                }
                user.Email = req.Email;
            }
            if (req.Password is not null)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, req.Password);

            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                res.HasError = true;
                res.ErrorMessage = $"An error ocurred trying to create the user";
                return res;
            }
            return res;
        }
        public async Task<List<UserDevelopersResponse>> GetAllDevelopersAsync()
        {
            var userlist = await _userManager.Users.ToListAsync();

            var adminlist = new List<UserDevelopersResponse>();
            foreach (var user in userlist)
            {
                if (await _userManager.IsInRoleAsync(user, Roles.Developer.ToString()))
                {
                    adminlist.Add(new UserDevelopersResponse
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IdCard = user.IdCard,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        IsActive = user.IsActive
                    });
                }

            }
            return adminlist;
        }
        public async Task<UserDevelopersResponse> GetDevelopersByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userResponse = new UserDevelopersResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IdCard = user.IdCard,
                UserName = user.UserName,
                IsActive = user.IsActive

            };

            return userResponse;
        }
        public async Task<bool> UpdateDevelopersStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);

            return true;
        }
        #endregion

        #region Client
        public async Task<UserResponse> GetClientByIdAsync(string clientId)
        {
            var user = await _userManager.FindByIdAsync(clientId);
            if (user == null) return null;

            return new UserResponse
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhotoUrl = user.PhotoUrl
            };
        }
        #endregion
    }
}
