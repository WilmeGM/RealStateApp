using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Services;
using System.Reflection;

namespace RealStateApp.Core.Application.Extensions
{
    public static class ServiceCollectionApplicationExtensions
    {
        public static void AddApplicationCore(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<ISaleTypeService, SaleTypeService>();
            services.AddTransient<IImprovementService, ImprovementService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IFavoritePropertyService, FavoritePropertyService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IChatMessageService, ChatMessageService>();
            services.AddTransient<IOfferService, OfferService>();
        }

        public static void AddApplicationCoreForWebApi(this IServiceCollection services)
        {
            AddApplicationCoreGenericConfiguration(services);
            AddApplicationCoreGenericServices(services);
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<ISaleTypeService, SaleTypeService>();
            services.AddTransient<IImprovementService, ImprovementService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IFavoritePropertyService, FavoritePropertyService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IChatMessageService, ChatMessageService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        private static void AddApplicationCoreGenericConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        private static void AddApplicationCoreGenericServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
        }
    }
}
