using HabilitationApp.Data;
using HabilitationApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES MVC
builder.Services.AddControllersWithViews();

// 2. CONFIGURATION ORACLE
var oracleConnectionString = builder.Configuration.GetConnectionString("OracleConnection");

if (string.IsNullOrEmpty(oracleConnectionString))
{
    Console.WriteLine("⚠️ Aucune chaîne de connexion Oracle trouvée dans appsettings.json");
    Console.WriteLine("🔄 Utilisation de la base de données en mémoire pour le développement...");
    
    // Fallback: base de données en mémoire
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("HabilitationDb"));
}
else
{
    Console.WriteLine("🔧 Configuration Oracle chargée");
    
    // Configuration Oracle
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(
            oracleConnectionString,
            oracleOptions => oracleOptions
                .UseOracleSQLCompatibility((OracleSQLCompatibility)11) // Ajustez selon votre version Oracle
        ));
}

// 3. INJECTION DES REPOSITORIES
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

var app = builder.Build();

// 4. TEST DE CONNEXION ORACLE
if (!string.IsNullOrEmpty(oracleConnectionString))
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        try
        {
            if (dbContext.Database.CanConnect())
            {
                Console.WriteLine("✅ Connexion Oracle établie avec succès !");
                
                // Optionnel: Appliquer les migrations automatiquement
                // await dbContext.Database.MigrateAsync();
            }
            else
            {
                Console.WriteLine("❌ Impossible de se connecter à Oracle");
                Console.WriteLine("💡 Vérifiez que:");
                Console.WriteLine("   1. Oracle Database est en cours d'exécution");
                Console.WriteLine("   2. Le service listener est actif (port 1521)");
                Console.WriteLine("   3. Les identifiants sont corrects");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erreur de connexion Oracle: {ex.Message}");
            Console.WriteLine("🔄 Utilisation de la base de données en mémoire...");
        }
    }
}
else
{
    Console.WriteLine("🎯 Utilisation de la base de données en mémoire pour le développement");
}

// 5. CONFIGURATION MIDDLEWARE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 6. ROUTING
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Utilisateur}/{action=Index}/{id?}");

// 7. DÉMARRAGE
Console.WriteLine("🚀 Application démarrée avec succès !");
Console.WriteLine($"📱 URL: {app.Urls.FirstOrDefault() ?? "http://localhost:5128"}");
Console.WriteLine("👤 Test des utilisateurs: /Utilisateur");
Console.WriteLine("");

app.Run();