using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IImprovementRepository : IGenericRepository<Improvement>
    {
        Task<Improvement> GetImprovementByIdAsync(int improvementId);
    }
}
