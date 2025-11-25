using HabilitationApp.Data;
using HabilitationApp.Repositories;
using HabilitationApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext - Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Repositories & Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
builder.Services.AddScoped<INotificationService, EmailNotificationService>();

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
