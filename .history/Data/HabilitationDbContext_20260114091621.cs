using Microsoft.EntityFrameworkCore;
using CGB_Habilitation.Models;

namespace CGB_Habilitation.Data
{
    public class HabilitationDbContext : DbContext
    {
        public HabilitationDbContext(DbContextOptions<HabilitationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Agence> Agences { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Habilitation> Habilitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Agence
            modelBuilder.Entity<Agence>(entity =>
            {
                entity.HasKey(a => a.CodeAgence);
                entity.Property(a => a.CodeAgence)
                    .HasDefaultValueSql("SYS_GUID()");
                
                entity.Property(a => a.NomAgence)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(a => a.AdresseAgence)
                    .HasMaxLength(200);
                
                entity.HasIndex(a => a.NomAgence)
                    .IsUnique();
            });

            // Configure Service
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(s => s.CodeService);
                entity.Property(s => s.CodeService)
                    .HasDefaultValueSql("SYS_GUID()");
                
                entity.Property(s => s.NomService)
                    .HasMaxLength(100);
                
                entity.Property(s => s.TypeService)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasConversion(
                        v => v.ToUpper(),
                        v => v
                    );
                
                entity.HasCheckConstraint("CK_Service_Type", 
                    "TypeService IN ('BO', 'FO')");
                
                entity.HasIndex(s => s.NomService)
                    .IsUnique();
            });

            // Configure Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.IdRole);
                entity.Property(r => r.IdRole)
                    .HasDefaultValueSql("SYS_GUID()");
                
                entity.Property(r => r.NomRole)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.HasIndex(r => r.NomRole)
                    .IsUnique();
            });

            // Configure Agent
            modelBuilder.Entity<Agent>(entity =>
            {
                entity.HasKey(a => a.IdAgent);
                entity.Property(a => a.IdAgent)
                    .HasDefaultValueSql("SYS_GUID()");
                
                // Properties configuration
                entity.Property(a => a.CodeAgent)
                    .IsRequired()
                    .HasMaxLength(20);
                
                entity.Property(a => a.NomAgent)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(a => a.PrenomAgent)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(a => a.EmailAgent)
                    .HasMaxLength(100);
                
                entity.Property(a => a.LoginAgent)
                    .IsRequired()
                    .HasMaxLength(30);
                
                entity.Property(a => a.PasswordAgent)
                    .IsRequired()
                    .HasMaxLength(255); // For hashed passwords
                
                // Boolean to NUMBER conversion for Oracle
                entity.Property(a => a.SousCaisseAgent)
                    .HasConversion<int>()
                    .HasColumnType("NUMBER(1)")
                    .HasDefaultValue(0);
                
                entity.Property(a => a.EstValide)
                    .HasConversion<int>()
                    .HasColumnType("NUMBER(1)")
                    .HasDefaultValue(0);
                
                // Date configurations
                entity.Property(a => a.DateLogin)
                    .HasColumnType("TIMESTAMP(0)");
                
                // Indexes
                entity.HasIndex(a => a.CodeAgent)
                    .IsUnique();
                
                entity.HasIndex(a => a.LoginAgent)
                    .IsUnique();
                
                entity.HasIndex(a => a.EmailAgent)
                    .IsUnique()
                    .HasFilter("[EmailAgent] IS NOT NULL");
                
                entity.HasIndex(a => new { a.NomAgent, a.PrenomAgent });
                
                // Relationships
                entity.HasOne(a => a.Agence)
                    .WithMany()
                    .HasForeignKey(a => a.CodeAgence)
                    .HasPrincipalKey(ag => ag.CodeAgence)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(a => a.Service)
                    .WithMany()
                    .HasForeignKey(a => a.CodeService)
                    .HasPrincipalKey(s => s.CodeService)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(a => a.RoleAgent)
                    .WithMany()
                    .HasForeignKey(a => a.IdRole)
                    .HasPrincipalKey(r => r.IdRole)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // Check constraint for SousCaisseAgent
                entity.HasCheckConstraint("CK_Agent_SousCaisse", 
                    "SousCaisseAgent IN (0, 1)");
                
                entity.HasCheckConstraint("CK_Agent_EstValide", 
                    "EstValide IN (0, 1)");
            });

            // Configure Habilitation
            modelBuilder.Entity<Habilitation>(entity =>
            {
                entity.HasKey(h => h.IdHabilitation);
                entity.Property(h => h.IdHabilitation)
                    .HasDefaultValueSql("SYS_GUID()");
                
                // Properties configuration
                entity.Property(h => h.TypeHabilitation)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(h => h.Etat)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("En attente")
                    .HasConversion(
                        v => v.ToUpper(),
                        v => v
                    );
                
                entity.Property(h => h.DateCreationDemande)
                    .HasColumnType("TIMESTAMP(0)")
                    .HasDefaultValueSql("SYSDATE");
                
                entity.Property(h => h.DateDebut)
                    .HasColumnType("TIMESTAMP(0)");
                
                entity.Property(h => h.DateFin)
                    .HasColumnType("TIMESTAMP(0)");
                
                entity.Property(h => h.Motif)
                    .IsRequired()
                    .HasMaxLength(500);
                
                entity.Property(h => h.PieceJoint)
                    .HasMaxLength(255);
                
                // Check constraints
                entity.HasCheckConstraint("CK_Habilitation_Etat", 
                    "Etat IN ('EN ATTENTE', 'APPROUVÉE', 'REJETÉE', 'EXPIRÉE')");
                
                entity.HasCheckConstraint("CK_Habilitation_Dates", 
                    "DateFin IS NULL OR DateDebut <= DateFin");
                
                // Indexes
                entity.HasIndex(h => h.IdAgent);
                entity.HasIndex(h => h.Etat);
                entity.HasIndex(h => h.DateCreationDemande);
                entity.HasIndex(h => h.DateDebut);
                entity.HasIndex(h => h.DateFin);
                entity.HasIndex(h => h.CreerPar);
                entity.HasIndex(h => h.TraiterPar);
                
                // Relationships
                entity.HasOne(h => h.Agent)
                    .WithMany(a => a.Habilitations)
                    .HasForeignKey(h => h.IdAgent)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Self-referencing relationships for CreerPar and TraiterPar
                entity.HasOne<Agent>()
                    .WithMany()
                    .HasForeignKey(h => h.CreerPar)
                    .HasPrincipalKey(a => a.IdAgent)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne<Agent>()
                    .WithMany()
                    .HasForeignKey(h => h.TraiterPar)
                    .HasPrincipalKey(a => a.IdAgent)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default roles
            var adminRoleId = Guid.NewGuid();
            var chefAgenceRoleId = Guid.NewGuid();
            var agentRoleId = Guid.NewGuid();
            
            modelBuilder.Entity<Role>().HasData(
                new Role { IdRole = adminRoleId, NomRole = "Administrateur" },
                new Role { IdRole = chefAgenceRoleId, NomRole = "Chef d'Agence" },
                new Role { IdRole = agentRoleId, NomRole = "Agent" }
            );

            // Seed default service types
            var serviceBOId = Guid.NewGuid();
            var serviceFOId = Guid.NewGuid();
            
            modelBuilder.Entity<Service>().HasData(
                new Service { 
                    CodeService = serviceBOId, 
                    NomService = "Back Office", 
                    TypeService = "BO" 
                },
                new Service { 
                    CodeService = serviceFOId, 
                    NomService = "Front Office", 
                    TypeService = "FO" 
                }
            );

            // Seed default agence
            var defaultAgenceId = Guid.NewGuid();
            modelBuilder.Entity<Agence>().HasData(
                new Agence { 
                    CodeAgence = defaultAgenceId, 
                    NomAgence = "Siège Central", 
                    AdresseAgence = "123 Avenue Principale, Ville" 
                }
            );
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Auto-set dates for certain entities
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Habilitation && 
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((Habilitation)entityEntry.Entity).DateCreationDemande = DateTime.Now;
                }
                
                // Auto-update dates when state changes
                var habilitation = (Habilitation)entityEntry.Entity;
                if (habilitation.Etat == "APPROUVÉE" && habilitation.DateDebut == null)
                {
                    habilitation.DateDebut = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}