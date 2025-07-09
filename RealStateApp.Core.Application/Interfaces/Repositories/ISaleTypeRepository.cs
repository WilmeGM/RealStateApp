using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface ISaleTypeRepository : IGenericRepository<SaleType>
    {
        Task<int> GetPropertyCountForTypeAsync(int propertyTypeId);
    }
}
