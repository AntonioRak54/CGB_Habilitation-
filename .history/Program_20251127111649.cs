using HabilitationApp.Data;
using HabilitationApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Use in-memory database for development
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("HabilitationDb"));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

var app = builder.Build();

Console.WriteLine("🎯 Using IN-MEMORY database for development");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Utilisateur}/{action=Index}/{id?}");

Console.WriteLine("🚀 Application started successfully!");
Console.WriteLine("📱 Open: http://localhost:5128");
Console.WriteLine("👤 Test: http://localhost:5128/Utilisateur");

app.Run();