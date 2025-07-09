using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<List<Property>> GetPropertiesByAgentIdAsync(string agentId);
        Task<List<Property>> GetAvailablePropertiesAsync();
        Task<int> GetPropertyCountByAgentId(string agentId);
    }
}
