using RealStateApp.Infrastructure.Persistence.Extensions;
using RealStateApp.Core.Application.Extensions;
using RealStateApp.Infrastructure.Identity.Extensions;
using RealStateApp.Presentation.WebApp.Middlewares;
using RealStateApp.Infrastructure.Shared.Extensions;

namespace RealStateApp.Presentation.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddSession();
            builder.Services.AddApplicationCore();
            builder.Services.AddSharedInfrastructure(builder.Configuration);
            builder.Services.AddPersistenceInfrastructure(builder.Configuration);
            builder.Services.AddIdentityInfrastructure(builder.Configuration);
            builder.Services.AddScoped<LoginAuthorize>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            try
            {
                var app = builder.Build();

                await app.Services.RunIdentitySeedsAsync();

                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                    app.UseStatusCodePagesWithReExecute("/Error/{0}");
                    app.UseHsts();
                }

                app.UseSession();
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                await app.RunAsync();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
