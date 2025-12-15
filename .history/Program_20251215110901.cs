using Microsoft.EntityFrameworkCore;
using CGB_Habilitation.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Oracle connection string (UTILISATEUR NORMAL)
var oracleConnectionString =
    "User Id=CGB_USER;Password=cgb123;Data Source=localhost:1521/XE;";

builder.Services.AddDbContext<HabilitationDbContext>(options =>
    options.UseOracle(oracleConnectionString));

builder.Services.AddControllersWithViews();

// (Optionnel plus tard)
// builder.Services.AddAuthentication();
// builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();
