using RealStateApp.Core.Application.Dtos.Error;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.User
{
    public class UpdatePropertyTypeViewModel : BaseErrorReportEntity
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
