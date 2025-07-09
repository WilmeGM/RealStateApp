namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, UpdateViewModel, ViewModel, Model>
           where SaveViewModel : class
           where ViewModel : class
           where Model : class
    {
        Task<SaveViewModel> AddAsync(SaveViewModel vm);
        Task UpdateAsync(UpdateViewModel vm, int id);
        Task DeleteAsync(int id);
        Task<SaveViewModel> GetByIdSaveViewModelAsync(int id);
        Task<List<ViewModel>> GetAllViewModelAsync();
    }
}
