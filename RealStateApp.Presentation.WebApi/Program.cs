using RealStateApp.Core.Application.Extensions;
using RealStateApp.Infrastructure.Identity.Extensions;
using RealStateApp.Infrastructure.Persistence.Extensions;
using RealStateApp.Presentation.WebApi.Extensions;
using RealStateApp.Infrastructure.Shared.Extensions;

namespace RealStateApp.Presentation.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddApplicationCoreForWebApi();
            builder.Services.AddSharedInfrastructure(builder.Configuration);
            builder.Services.AddPersistenceInfrastructure(builder.Configuration);
            builder.Services.AddIdentityInfrastructureForWebApi(builder.Configuration);
            builder.Services.AddHealthChecks();
            builder.Services.AddSwaggerExtension();
            builder.Services.AddApiVersioningExtension();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseHealthChecks("/health");
            app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}
