using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyService : IGenericService<SavePropertyViewModel, SavePropertyViewModel, PropertyViewModel, Property>
    {
        Task<List<PropertyViewModel>> GetAvailablePropertiesByAgentIdAsync(string agentId);
        Task<List<PropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId);
        Task<List<PropertyViewModel>> GetAvailablePropertiesAsync();
        Task<PropertyViewModel> GetPropertyViewModelByIdAsync(int id);
        Task<(int Available, int Sold)> GetPropertyStatusCountsAsync();
    }
}
