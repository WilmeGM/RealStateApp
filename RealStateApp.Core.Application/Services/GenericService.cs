using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;


namespace RealStateApp.Core.Application.Services
{
    public class GenericService<SaveViewModel,UpdateViewModel, ViewModel, Entity> : IGenericService<SaveViewModel,UpdateViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        private readonly IGenericRepository<Entity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<List<ViewModel>> GetAllViewModelAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<ViewModel>>(entities);
        }

        public virtual async Task<SaveViewModel> GetByIdSaveViewModelAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<SaveViewModel>(entity);
        }

        public virtual async Task<SaveViewModel> AddAsync(SaveViewModel vm)
        {
            var entity = _mapper.Map<Entity>(vm);
            entity = await _repository.AddAsync(entity);
            return _mapper.Map<SaveViewModel>(entity);
        }

        public virtual async Task UpdateAsync(UpdateViewModel vm, int id)
        {
            var entity = _mapper.Map<Entity>(vm);
            await _repository.UpdateAsync(entity, id);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            await _repository.RemoveAsync(entity);
        }
    }
}
