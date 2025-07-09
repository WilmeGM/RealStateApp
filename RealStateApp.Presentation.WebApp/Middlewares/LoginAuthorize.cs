using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RealStateApp.Infrastructure.Identity.Entities;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Dtos.Account;

namespace RealStateApp.Presentation.WebApp.Middlewares
{
    public class LoginAuthorize(UserManager<ApplicationUser> userManager) : IAsyncActionFilter
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var session = context.HttpContext.Session.Get<LoginResponse>("user");

            if (session == null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = context.HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);

                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    session = new LoginResponse
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Roles = roles.ToList(),
                        IsActive = user.IsActive
                    };

                    context.HttpContext.Session.Set("user", session);
                }
            }

            if (session != null)
            {
                if (session.Roles.Contains(Roles.Admin.ToString()))
                {
                    context.Result = new RedirectToRouteResult(new { controller = "Admin", action = "Index" });
                    return;
                }
                else if (session.Roles.Contains(Roles.Client.ToString()))
                {
                    context.Result = new RedirectToRouteResult(new { controller = "Client", action = "Index" });
                    return;
                }
                else if (session.Roles.Contains(Roles.Agent.ToString()))
                {
                    context.Result = new RedirectToRouteResult(new { controller = "Agent", action = "Index" });
                    return;
                }
                else if (session.Roles.Contains(Roles.Developer.ToString()))
                {
                    context.Result = new RedirectToRouteResult(new { controller = "Developer", action = "Index" });
                    return;
                }
            }

            await next();
        }
    }
}
