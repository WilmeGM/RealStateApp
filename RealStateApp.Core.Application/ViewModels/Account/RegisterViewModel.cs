using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Error;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Account
{
    public class RegisterViewModel : BaseErrorReportEntity
    {
        public string? Id { get; set; }
        public string? PhotoUrl { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Passsword is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Passsword confirm is required")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}
