using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Infrastructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RealStateApp.Infrastructure.Identity.Services
{
    public class AccountServiceForWebApi(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<JWTSettings> jWTSettings) : IAccountServiceForWebApi
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly JWTSettings _jwtSettings = jWTSettings.Value;

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            LoginResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.ErrorMessage = $"No Accounts registered with {request.Email}";
                return response;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                response.HasError = true;
                response.ErrorMessage = $"Invalid password for {request.Email}";
                return response;
            }

            if (await _userManager.IsInRoleAsync(user, Roles.Agent.ToString()))
            {
                response.HasError = true;
                response.ErrorMessage = "No puedes acceder como agente a la api";
                return response;
            } else if (await _userManager.IsInRoleAsync(user, Roles.Client.ToString()))
            {
                response.HasError = true;
                response.ErrorMessage = "No puedes acceder como cliente a la api";
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsActive = user.IsActive;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;
        }

        public async Task<CreateAdminUserResponse> RegisterAdminUserAsync(CreateAdminUserRequest req)
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

        public async Task<CreateDevelopersUserResponse> RegisterDeveloperUserAsync(CreateDevelopersUserRequest req)
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

        #region Private Methods
        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            //obtengo los datos del usuario (claims)
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            //creo y agrego claims para cada role con la clave roles
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id)
            }.Union(userClaims).Union(roleClaims);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredetials);

            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var ramdomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(ramdomBytes);

            return BitConverter.ToString(ramdomBytes).Replace("-", "");
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }
        #endregion
    }
}
