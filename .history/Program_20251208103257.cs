using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oracle.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration de la connexion Oracle XE
// Remplacez les valeurs selon votre configuration Docker
var connectionString = builder.Configuration.GetConnectionString("OracleConnection") 
    ?? "Data Source=localhost:1521/XE;User Id=SYSTEM;Password=YourPassword123;";

// Ajout des services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CGB Habilitation API", Version = "v1" });
});

// Configuration d'Entity Framework Core pour Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(connectionString));

var app = builder.Build();

// Configuration du pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Migration automatique (à utiliser avec précaution en production)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Cette ligne crée la base si elle n'existe pas et applique les migrations
        dbContext.Database.EnsureCreated();
        Console.WriteLine("✅ Connexion à Oracle XE établie avec succès!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erreur de connexion à Oracle: {ex.Message}");
    }
}

app.Run();

// Modèle de DbContext minimal
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // Ajoutez vos DbSet ici
    // public DbSet<User> Users { get; set; }
    // public DbSet<Role> Roles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configuration des modèles ici
    }
}

// Exemple de modèle
public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}