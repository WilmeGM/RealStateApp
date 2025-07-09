using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Infrastructure.Shared.Services;

namespace RealStateApp.Infrastructure.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
