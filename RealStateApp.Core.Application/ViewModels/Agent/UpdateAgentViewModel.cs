using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Error;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Agent
{
    public class UpdateAgentViewModel : BaseErrorReportEntity
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Photo { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
