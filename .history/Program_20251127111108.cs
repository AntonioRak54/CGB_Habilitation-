using HabilitationApp.Data;
using HabilitationApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Try Oracle first, fallback to in-memory if it fails
bool oracleConnected = false;

try
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));
    oracleConnected = true;
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Oracle connection failed, using in-memory database: {ex.Message}");
}

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

var app = builder.Build();

// Test connection only if using Oracle
if (oracleConnected)
{
    using (var scope = app.Services.CreateScope())
    {
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            if (ctx.Database.CanConnect())
            {
                Console.WriteLine("✅ Connexion Oracle OK !");
            }
            else
            {
                Console.WriteLine("❌ ERREUR ORACLE: Cannot connect to database");
                Environment.Exit(1);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ ERREUR ORACLE: {ex.Message}");
            Environment.Exit(1);
        }
    }
}
else
{
    Console.WriteLine("ℹ️ Using in-memory database for development");
}

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

app.Run();