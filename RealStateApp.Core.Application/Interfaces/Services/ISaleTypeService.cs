using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface ISaleTypeService : IGenericService<SaveSaleTypeViewModel, UpdateSaleTypeViewModel, SaleTypeViewModel, SaleType>
    {
    }
}
