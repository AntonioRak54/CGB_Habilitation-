using HabilitationApp.Data;
using HabilitationApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ======================================
// 1. SERVICES MVC
// ======================================
builder.Services.AddControllersWithViews();

// ======================================
// 2. CONNEXION ORACLE (EF CORE)
// ======================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// ======================================
// 3. INJECTION DES REPOSITORIES & SERVICES
// ======================================
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUtilisateurRepository, IUtilisateurRepository>();
// builder.Services.AddScoped<INotificationService, EmailNotificationService>();

var app = builder.Build();

// ======================================
// 4. TEST DE CONNEXION À ORACLE
// ======================================
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        ctx.Database.CanConnect();
        Console.WriteLine("🔥 Connexion Oracle OK !");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERREUR ORACLE ❌");
        Console.WriteLine(ex.Message);
    }
}

// ======================================
// 5. PIPELINE MIDDLEWARE
// ======================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ======================================
// 6. ROUTING MVC
// ======================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Utilisateur}/{action=Index}/{id?}");

// ======================================
// 7. RUN
// ======================================
app.Run();
