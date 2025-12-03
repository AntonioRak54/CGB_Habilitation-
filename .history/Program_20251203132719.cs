using HabilitationApp.Data;
using HabilitationApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// CONFIGURATION ORACLE DANS DOCKER
var oracleConnectionString = builder.Configuration.GetConnectionString("OracleConnection");

if (string.IsNullOrEmpty(oracleConnectionString))
{
    Console.WriteLine("⚠️ Aucune chaîne de connexion Oracle trouvée");
    Console.WriteLine("💡 Pour Docker Oracle, utilisez cette configuration dans appsettings.json:");
    Console.WriteLine(@"
    ""ConnectionStrings"": {
        ""OracleConnection"": ""User Id=system;Password=Oracle123;Data Source=localhost:1521/XEPDB1;""
    }");
    
    // Configuration par défaut pour Docker Oracle
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle("User Id=system;Password=Oracle123;Data Source=localhost:1521/XEPDB1;"));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(oracleConnectionString));
}

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

var app = builder.Build();

// TEST DE CONNEXION
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    try
    {
        // Attendre un peu pour laisser Oracle démarrer complètement
        await Task.Delay(2000);
        
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("✅ Connexion Oracle Docker établie !");
            
            // Créer la base de données si elle n'existe pas
            await dbContext.Database.EnsureCreatedAsync();
            Console.WriteLine("✅ Base de données prête");
        }
        else
        {
            Console.WriteLine("❌ Impossible de se connecter à Oracle Docker");
            Console.WriteLine("📋 Vérifiez que:");
            Console.WriteLine("   1. Le conteneur Oracle est en cours d'exécution");
            Console.WriteLine("   2. Le port 1521 est exposé: docker run -p 1521:1521 ...");
            Console.WriteLine("   3. Vous utilisez le bon mot de passe");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erreur Oracle: {ex.Message}");
        Console.WriteLine("💡 Solutions possibles:");
        Console.WriteLine("   - Vérifiez que Oracle Docker est démarré");
        Console.WriteLine("   - Attendez qu'Oracle soit complètement démarré (peut prendre 2-3 minutes)");
        Console.WriteLine("   - Utilisez XEPDB1 au lieu de XE pour les images récentes");
    }
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

Console.WriteLine("🚀 Application démarrée !");
Console.WriteLine("📱 URL: http://localhost:5128");

app.Run();