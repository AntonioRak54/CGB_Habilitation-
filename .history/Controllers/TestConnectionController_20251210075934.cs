using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace CGB_Habilitation.Controllers
{
    public class TestConnectionController : Controller
    {
        private readonly IConfiguration _configuration;

        public TestConnectionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var connectionString = "User Id=sys;Password=antoniodb;Data Source=localhost:1521/XE;DBA Privilege=SYSDBA;";
            
            try
            {
                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT 'Connexion réussie!' as message FROM DUAL";
                        var result = command.ExecuteScalar();
                        
                        ViewBag.Message = $"Connexion Oracle réussie! Message: {result}";
                        ViewBag.ConnectionState = connection.State;
                        ViewBag.ServerVersion = connection.ServerVersion;
                    }
                }
                
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erreur de connexion: {ex.Message}";
                return View();
            }
        }
    }
}