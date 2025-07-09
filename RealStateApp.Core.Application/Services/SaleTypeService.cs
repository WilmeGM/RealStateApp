using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;


namespace RealStateApp.Core.Application.Services
{
    public class SaleTypeService : GenericService<SaveSaleTypeViewModel, UpdateSaleTypeViewModel, SaleTypeViewModel, SaleType>, ISaleTypeService
    {
        private readonly ISaleTypeRepository _repository;
        private readonly IMapper _mapper;

        public SaleTypeService(ISaleTypeRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Obtener todos los tipos de propiedad
        public async Task<List<SaleTypeViewModel>> GetAllAsync()
        {
            var saleTypes = await _repository.GetAllAsync();
            var saleTypeViewModels = _mapper.Map<List<SaleTypeViewModel>>(saleTypes);

            // Aquí añadimos la cantidad de propiedades asociadas a cada tipo de propiedad
            foreach (var saleType in saleTypeViewModels)
            {
                saleType.PropertyCount = await _repository.GetPropertyCountForTypeAsync(saleType.Id);
            }

            return saleTypeViewModels;
        }

        // Obtener un tipo de propiedad por su ID
        public async Task<SaleTypeViewModel> GetByIdAsync(int id)
        {
            var saleType = await _repository.GetByIdAsync(id);
            return _mapper.Map<SaleTypeViewModel>(saleType);
        }
    }
}
