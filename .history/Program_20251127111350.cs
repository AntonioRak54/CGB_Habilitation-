using HabilitationApp.Data;
using HabilitationApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Try Oracle first, fallback to in-memory if it fails
bool useInMemoryDatabase = false;

try
{
    var oracleConnectionString = builder.Configuration.GetConnectionString("OracleConnection");
    if (!string.IsNullOrEmpty(oracleConnectionString))
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseOracle(oracleConnectionString));
        Console.WriteLine("🔧 Oracle configuration loaded");
    }
    else
    {
        throw new Exception("Oracle connection string is empty");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Oracle configuration failed: {ex.Message}");
    Console.WriteLine("🔄 Switching to in-memory database for development...");
    
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("HabilitationDb"));
    useInMemoryDatabase = true;
}

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

var app = builder.Build();

// Test database connection only if using Oracle
if (!useInMemoryDatabase)
{
    using (var scope = app.Services.CreateScope())
    {
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            if (ctx.Database.CanConnect())
            {
                Console.WriteLine("Connexion Oracle OK !");
            }
            else
            {
                Console.WriteLine("ERREUR ORACLE: Cannot connect to database");
                Console.WriteLine("Switching to in-memory database...");
                
                // Rebuild with in-memory database
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("HabilitationDb"), ServiceLifetime.Scoped);
                useInMemoryDatabase = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERREUR ORACLE: {ex.Message}");
            Console.WriteLine("Switching to in-memory database...");
            
            // Rebuild with in-memory database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("HabilitationDb"), ServiceLifetime.Scoped);
            useInMemoryDatabase = true;
        }
    }
}

if (useInMemoryDatabase)
{
    Console.WriteLine("Using IN-MEMORY database for development");
    Console.WriteLine("You can test all MVC features without Oracle");
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

// Seed some test data if using in-memory database
if (useInMemoryDatabase)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // You can add test data here if needed
    }
}

Console.WriteLine("Application started successfully!");
Console.WriteLine("Open: http://localhost:5128");
Console.WriteLine("Test: http://localhost:5128/Utilisateur");

app.Run();