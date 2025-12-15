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

            // Configure primary keys explicitly
            modelBuilder.Entity<Agence>()
                .HasKey(a => a.CodeAgence);  // Explicitly set CodeAgence as PK

            modelBuilder.Entity<Service>()
                .HasKey(s => s.CodeService);  // Explicitly set CodeService as PK

            modelBuilder.Entity<Role>()
                .HasKey(r => r.IdRole);  // This one follows convention but we set it explicitly anyway

            modelBuilder.Entity<Agent>()
                .HasKey(a => a.IdAgent);

            modelBuilder.Entity<Habilitation>()
                .HasKey(h => h.IdHabilitation);

            // Configure relationships
            modelBuilder.Entity<Agent>()
                .HasOne(a => a.Agence)
                .WithMany()  // If Agence doesn't have Agents collection, use WithMany()
                .HasForeignKey(a => a.CodeAgence)
                .HasPrincipalKey(ag => ag.CodeAgence);  // Reference CodeAgence as foreign key

            modelBuilder.Entity<Agent>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.CodeService)
                .HasPrincipalKey(s => s.CodeService);  // Reference CodeService as foreign key

            modelBuilder.Entity<Agent>()
                .HasOne(a => a.Role)
                .WithMany()
                .HasForeignKey(a => a.IdRole)
                .HasPrincipalKey(r => r.IdRole);

            modelBuilder.Entity<Habilitation>()
                .HasOne(h => h.Agent)
                .WithMany(a => a.Habilitations)
                .HasForeignKey(h => h.IdAgent);
        }
    }
}