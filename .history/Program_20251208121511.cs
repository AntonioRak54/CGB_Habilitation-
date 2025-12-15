using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CGB_Habilitation.Data;

namespace CGB_Habilitation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Test de connexion à Oracle XE ===");
            
            try
            {
                // 1. Charger la configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // 2. Récupérer la chaîne de connexion
                var connectionString = configuration.GetConnectionString("OracleConnection");
                Console.WriteLine($"Chaîne de connexion : {connectionString}");
                
                // 3. Configurer DbContext
                var optionsBuilder = new DbContextOptionsBuilder<HabilitationDbContext>();
                optionsBuilder.UseOracle(connectionString);
                
                // 4. Tester la connexion
                using (var context = new HabilitationDbContext(optionsBuilder.Options))
                {
                    Console.WriteLine("\nTentative de connexion à la base de données...");
                    
                    // Tester si on peut se connecter
                    bool canConnect = context.Database.CanConnect();
                    
                    if (canConnect)
                    {
                        Console.WriteLine("Connexion réussie à Oracle XE !");
                        
                        // Afficher des informations sur la base
                        var connection = context.Database.GetDbConnection();
                        Console.WriteLine($"\nInformations de connexion :");
                        Console.WriteLine($"- Serveur : {connection.DataSource}");
                        Console.WriteLine($"- Base de données : {connection.Database}");
                        Console.WriteLine($"- État : {connection.State}");
                        
                        // Tester une requête simple
                        Console.WriteLine("\nTest de requête SQL...");
                        var result = context.Database.SqlQueryRaw<int>("SELECT 1 FROM DUAL").FirstOrDefault();
                        Console.WriteLine($"Résultat du test : {result}");
                        
                        if (result == 1)
                        {
                            Console.WriteLine("Requête SQL fonctionnelle !");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Impossible de se connecter à la base de données.");
                        Console.WriteLine("Vérifiez que :");
                        Console.WriteLine("1. Oracle XE est en cours d'exécution (docker ps)");
                        Console.WriteLine("2. Le port 1521 est accessible");
                        Console.WriteLine("3. Les identifiants dans appsettings.json sont corrects");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur : {ex.Message}");
                
                // Afficher plus de détails pour le débogage
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"\nDétails : {ex.InnerException.Message}");
                }
                
                Console.WriteLine("\nStack trace :");
                Console.WriteLine(ex.StackTrace);
            }
            
            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }

    internal class HabilitationDbContext : IDisposable
    {
    }
}