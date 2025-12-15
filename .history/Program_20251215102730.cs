using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

var oracleConnectionString = "User Id=sys;Password=antoniobd;Data Source=localhost:1521/XE;DBA Privilege=SYSDBA;";

builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connection = new OracleConnection(oracleConnectionString);
    connection.Open();
    return connection;
});

builder.Services.AddControllersWithViews();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();