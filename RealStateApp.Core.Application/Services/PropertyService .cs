using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Enums;

namespace RealStateApp.Core.Application.Services
{
    public class PropertyService(IPropertyRepository propertyRepository,
        IMapper mapper,
        IImprovementRepository improvementRepository) :
        GenericService<
            SavePropertyViewModel,
            SavePropertyViewModel,
            PropertyViewModel,
            Property>(propertyRepository, mapper), IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository = propertyRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IImprovementRepository _improvementRepository = improvementRepository;

        public async Task<List<PropertyViewModel>> GetAvailablePropertiesByAgentIdAsync(string agentId)
        {
            var properties = await _propertyRepository.GetPropertiesByAgentIdAsync(agentId);
            var availableProperties = properties.Where(p => p.PropertyStatus == "Available").ToList();
            return _mapper.Map<List<PropertyViewModel>>(availableProperties);
        }

        public async Task<List<PropertyViewModel>> GetAvailablePropertiesAsync()
        {
            var properties = await _propertyRepository.GetAvailablePropertiesAsync();
            return _mapper.Map<List<PropertyViewModel>>(properties);
        }


        public override async Task<SavePropertyViewModel> AddAsync(SavePropertyViewModel viewModel)
        {
            var property = _mapper.Map<Property>(viewModel);
            property.UniqueCode = IdGeneratorHelper.GenerateUniqueCode();
            property.PropertyStatus = PropertyStatus.Available.ToString();

            // Asignar las mejoras
            property.Improvements = [];
            foreach (var improvementId in viewModel.ImprovementIds)
            {
                var improvement = await _improvementRepository.GetImprovementByIdAsync(improvementId);
                if (improvement != null) property.Improvements.Add(improvement);
            }

            // Subir las imágenes y asignar las rutas
            property.ImageUrl1 = FileUploaderHelper.UploadImage(viewModel.Images.ElementAtOrDefault(0), viewModel.UserId, 1);
            property.ImageUrl2 = FileUploaderHelper.UploadImage(viewModel.Images.ElementAtOrDefault(1), viewModel.UserId, 2);
            property.ImageUrl3 = FileUploaderHelper.UploadImage(viewModel.Images.ElementAtOrDefault(2), viewModel.UserId, 3);
            property.ImageUrl4 = FileUploaderHelper.UploadImage(viewModel.Images.ElementAtOrDefault(3), viewModel.UserId, 4);

            property = await _propertyRepository.AddAsync(property);

            return _mapper.Map<SavePropertyViewModel>(property);
        }

        public override async Task UpdateAsync(SavePropertyViewModel viewModel, int id)
        {
            var property = await _propertyRepository.GetByIdAsync(viewModel.Id) ?? throw new Exception("Property not found.");

            // Update property details
            property.PropertyTypeId = viewModel.PropertyTypeId;
            property.SaleTypeId = viewModel.SaleTypeId;
            property.Description = viewModel.Description;
            property.Price = viewModel.Price;
            property.RoomCount = viewModel.RoomCount;
            property.BathroomCount = viewModel.BathroomCount;
            property.SizeInMeters = viewModel.SizeInMeters;

            property.Improvements.Clear(); // Elimina las mejoras actuales
            foreach (var improvementId in viewModel.ImprovementIds)
            {
                var improvement = await _improvementRepository.GetImprovementByIdAsync(improvementId);
                if (improvement != null)
                {
                    property.Improvements.Add(improvement);
                }
            }

            // Handle images    
            property.ImageUrl1 = FileUploaderHelper.UploadImage(viewModel.Images.ElementAtOrDefault(0), viewModel.UserId, 1, true, property.ImageUrl1);
            property.ImageUrl2 = FileUploaderHelper.UploadImage(viewModel.Images.ElementAtOrDefault(1), viewModel.UserId, 2, true, property.ImageUrl2);
            property.ImageUrl3 = FileUploaderHelper.UploadImage(viewModel.Images.ElementAtOrDefault(2), viewModel.UserId, 3, true, property.ImageUrl3);
            property.ImageUrl4 = FileUploaderHelper.UploadImage(viewModel.Images.ElementAtOrDefault(3), viewModel.UserId, 4, true, property.ImageUrl4);

            await _propertyRepository.UpdateAsync(property, id);
        }

        public async Task<List<PropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId)
        {
            var properties = await _propertyRepository.GetPropertiesByAgentIdAsync(agentId);
            var mappedProperties = _mapper.Map<List<PropertyViewModel>>(properties);

            foreach (var property in mappedProperties)
            {
                property.StatusLabel = property.PropertyStatus == PropertyStatus.Sold.ToString()
                    ? "Sold"
                    : "Available";
            }

            return mappedProperties;
        }

        public async Task<PropertyViewModel> GetPropertyViewModelByIdAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);

            if (property == null)
                return null;

            var propertyViewModel = _mapper.Map<PropertyViewModel>(property);

            return propertyViewModel;
        }

        public async Task<(int Available, int Sold)> GetPropertyStatusCountsAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();

            int availableCount = properties.Count(p => p.PropertyStatus == PropertyStatus.Available.ToString());
            int soldCount = properties.Count(p => p.PropertyStatus == PropertyStatus.Sold.ToString());

            return (availableCount, soldCount);
        }
    }
}
