using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Infrastructure.Persistence.Extensions
{
    public static class ServiceCollectionPersistenceExtension
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InternetBankingInMemory"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddTransient<ISaleTypeRepository, SaleTypeRepository>();
            services.AddTransient<IImprovementRepository, ImprovementRepository>();
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IFavoritePropertyRepository, FavoritePropertyRepository>();
            services.AddTransient<IChatMessageRepository, ChatMessageRepository>();
            services.AddTransient<IOfferRepository, OfferRepository>();
            #endregion
        }
    }
}
