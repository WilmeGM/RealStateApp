using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyTypeService : IGenericService<SavePropertyTypeViewModel,UpdatePropertyTypeViewModel, PropertyTypeViewModel, PropertyType>
    {
    }
}
