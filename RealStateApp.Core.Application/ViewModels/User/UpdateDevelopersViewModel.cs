using RealStateApp.Core.Application.Dtos.Error;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.User
{
    public class UpdateDevelopersViewModel : BaseErrorReportEntity
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "IdCard is required")]
        [DataType(DataType.Text)]
        public string IdCard { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }


        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Both Password must match")]

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
