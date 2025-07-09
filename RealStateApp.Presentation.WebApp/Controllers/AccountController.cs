using RealStateApp.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Account;
using RealStateApp.Presentation.WebApp.Middlewares;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Dtos.Account;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    public class AccountController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpGet]
        public IActionResult Index() => View(new LoginViewModel());

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel req)
        {
            if (!ModelState.IsValid) return View(req); 

            var result = await _userService.LoginAsync(req);
            if (result.HasError)
            {
                req.HasError = true;
                req.ErrorMessage = result.ErrorMessage;
                return View(req);
            }

            HttpContext.Session.Set("user", result);

            if (result.Roles.Contains(Roles.Admin.ToString()))
                return RedirectToRoute(new { controller = "Admin", action = "Index", loginSuccess = true });

            else if (result.Roles.Contains(Roles.Client.ToString()))
                return RedirectToRoute(new { controller = "Client", action = "Index", loginSuccess = true });

            else if (result.Roles.Contains(Roles.Agent.ToString()))
                return RedirectToRoute(new { controller = "Agent", action = "Index", loginSuccess = true });

            else if (result.Roles.Contains(Roles.Developer.ToString()))
                return RedirectToRoute(new { controller = "Admin", action = "Index", loginSuccess = true });

            return RedirectToAction("Login");
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpGet]
        public IActionResult Register()
            => View(new RegisterViewModel());

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var origin = Request.Headers.Origin;
            RegisterResponse response = await _userService.RegisterAsync(registerViewModel, origin);

            if (response.HasError)
            {
                registerViewModel.HasError = true;
                registerViewModel.ErrorMessage = response.ErrorMessage;
                return View(registerViewModel);
            }

            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        [HttpGet]
        public IActionResult AccessDenied()
            => View();

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}