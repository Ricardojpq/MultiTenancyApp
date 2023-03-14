using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MultiTenancyApp.Persistence;
using MultiTenancyApp.Services.Implementation;
using MultiTenancyApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;




namespace MultiTenancyApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,
          IConfiguration config, IWebHostEnvironment environment)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config["ConnectionStrings:SqlServer"]);
            });

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddTransient<IServiceTenant, ServiceTenant>();
            return services;
        }
    }
}
