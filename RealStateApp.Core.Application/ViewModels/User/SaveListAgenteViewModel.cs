using RealStateApp.Core.Application.Dtos.Error;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.User
{
    public class SaveListAgenteViewModel : BaseErrorReportEntity
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public int PropertyCount { get; set; }

    }
}
