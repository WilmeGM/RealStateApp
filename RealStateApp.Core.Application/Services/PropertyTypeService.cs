using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;


namespace RealStateApp.Core.Application.Services
{
    public class PropertyTypeService : GenericService<SavePropertyTypeViewModel,UpdatePropertyTypeViewModel, PropertyTypeViewModel, PropertyType>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _repository;
        private readonly IMapper _mapper;

        public PropertyTypeService(IPropertyTypeRepository repository, IMapper mapper) : base(repository,mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Obtener todos los tipos de propiedad
        public async Task<List<PropertyTypeViewModel>> GetAllAsync()
        {
            var propertyTypes = await _repository.GetAllAsync();
            var propertyTypeViewModels = _mapper.Map<List<PropertyTypeViewModel>>(propertyTypes);

            // Aquí añadimos la cantidad de propiedades asociadas a cada tipo de propiedad
            foreach (var propertyType in propertyTypeViewModels)
            {
                propertyType.PropertyCount = await _repository.GetPropertyCountForTypeAsync(propertyType.Id);
            }

            return propertyTypeViewModels;
        }

        // Obtener un tipo de propiedad por su ID
        public async Task<PropertyTypeViewModel> GetByIdAsync(int id)
        {
            var propertyType = await _repository.GetByIdAsync(id);
            return _mapper.Map<PropertyTypeViewModel>(propertyType);
        }

        // Crear un nuevo tipo de propiedad
       
    }
}

