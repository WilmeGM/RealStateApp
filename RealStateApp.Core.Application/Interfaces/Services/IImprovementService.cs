using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IImprovementService : IGenericService<SaveImprovementViewModel, UpdateImprovementViewModel, ImprovementViewModel, Improvement>
    {

    }
}
