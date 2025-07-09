using RealStateApp.Core.Application.Dtos.Error;
using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.User
{
    public class SavePropertyTypeViewModel : BaseErrorReportEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}
