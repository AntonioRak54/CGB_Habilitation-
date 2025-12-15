using System;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleConnectionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Début de la connexion Oracle XE...");

            // Chaîne de connexion Oracle XE
            string connectionString = GetConnectionString();
            
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("Tentative de connexion...");
                    
                    // Ouvrir la connexion
                    connection.Open();
                    Console.WriteLine("Connexion réussie !");
                    
                    // Afficher les informations de connexion
                    Console.WriteLine($"\nInformations de connexion:");
                    Console.WriteLine($"Base de données: {connection.Database}");
                    Console.WriteLine($"Serveur: {connection.DataSource}");
                    Console.WriteLine($"État: {connection.State}");
                    
                    // Exécuter une requête simple
                    ExecuteSampleQuery(connection);
                    
                    // Tester avec une requête sur les tables utilisateur
                    ListUserTables(connection);
                    
                }
                catch (OracleException ex)
                {
                    Console.WriteLine($"Erreur Oracle: {ex.Message}");
                    Console.WriteLine($"Code d'erreur: {ex.ErrorCode}");
                    Console.WriteLine($"Numéro d'erreur: {ex.Number}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur générale: {ex.Message}");
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        Console.WriteLine("\nConnexion fermée.");
                    }
                }
            }
            
            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }

        static string GetConnectionString()
        {
            // Configuration pour votre conteneur Docker Oracle XE
            // Pour l'utilisateur SYS (avec SYSDBA)
            return "User Id=sys;Password=antoniodb;Data Source=localhost:1521/XEPDB1;DBA Privilege=SYSDBA;";
            
            // Pour un utilisateur normal (si vous en créez un)
            // return "User Id=VOTRE_USER;Password=VOTRE_MDP;Data Source=localhost:1521/XEPDB1;";
        }

        static void ExecuteSampleQuery(OracleConnection connection)
        {
            try
            {
                Console.WriteLine("\n=== Exécution d'une requête de test ===");
                
                string query = "SELECT 'Hello from Oracle XE!' AS message FROM DUAL";
                
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Message: {reader["message"]}");
                        }
                    }
                }
                
                // Version avec paramètres
                Console.WriteLine("\n=== Version avec paramètre ===");
                string paramQuery = "SELECT :input AS result FROM DUAL";
                
                using (OracleCommand command = new OracleCommand(paramQuery, connection))
                {
                    command.Parameters.Add("input", OracleDbType.Varchar2).Value = "Test réussi!";
                    
                    var result = command.ExecuteScalar();
                    Console.WriteLine($"Résultat: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la requête: {ex.Message}");
            }
        }

        static void ListUserTables(OracleConnection connection)
        {
            try
            {
                Console.WriteLine("\n=== Tables de l'utilisateur ===");
                
                // Pour voir les tables sous le schéma SYS
                string query = @"
                    SELECT table_name, tablespace_name 
                    FROM all_tables 
                    WHERE owner = 'SYS' 
                    AND rownum <= 10
                    ORDER BY table_name";
                
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        int count = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine($"Table: {reader["table_name"]}, Tablespace: {reader["tablespace_name"]}");
                            count++;
                        }
                        Console.WriteLine($"Total: {count} tables");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la liste des tables: {ex.Message}");
            }
        }
    }
}