using RealStateApp.Core.Application.Dtos.Error;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Account
{
    public class LoginViewModel : BaseErrorReportEntity
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
