using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiTenancyApp;
using MultiTenancyApp.Persistence;
using MultiTenancyApp.Persistence.SeedData;
using MultiTenancyApp.Services.Implementation;
using MultiTenancyApp.Services.Interfaces;

AppSettings appSettings;
var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var appSettingsSection = configuration.GetSection("AppSettings");
appSettings = appSettingsSection.Get<AppSettings>();

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration,builder.Environment);
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


#region InicializacionDB
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>()
    .CreateScope())
{
    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
    context.Database.Migrate();

    if (appSettings.RunDbInitializer)
    {
        DbInitializer.Initialize(context, builder.Configuration).GetAwaiter().GetResult();
    }
}
#endregion

app.Run();
