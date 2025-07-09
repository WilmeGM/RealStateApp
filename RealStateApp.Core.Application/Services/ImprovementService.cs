using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Entities;


namespace RealStateApp.Core.Application.Services
{
    public class ImprovementService : GenericService<SaveImprovementViewModel, UpdateImprovementViewModel, ImprovementViewModel, Improvement>, IImprovementService
    {
        private readonly IImprovementRepository _repository;
        private readonly IMapper _mapper;

        public ImprovementService(IImprovementRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
