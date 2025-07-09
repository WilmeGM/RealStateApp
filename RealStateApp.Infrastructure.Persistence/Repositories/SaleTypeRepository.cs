using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories
{
    public class SaleTypeRepository : GenericRepository<SaleType>, ISaleTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SaleTypeRepository(ApplicationDbContext dbContext) : base(dbContext)

        {
            _dbContext = dbContext;
        }
        // Método para contar cuántas propiedades están asociadas a un tipo de propiedad
        public async Task<int> GetPropertyCountForTypeAsync(int saleTypeId)
        {
            return await _dbContext.Properties
                .Where(p => p.SaleTypeId == saleTypeId)
                .CountAsync();
        }
    }
}
