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
            modelBuilder.Entity<Agence>()
                .HasKey(a => a.CodeAgence);

            modelBuilder.Entity<Service>()
                .HasKey(s => s.CodeService);

            modelBuilder.Entity<Role>()
                .HasKey(r => r.IdRole);

            modelBuilder.Entity<Agent>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Habilitation>()
                .HasKey(h => h.IdHabilitation);

            // Configure boolean to NUMBER conversion
            modelBuilder.Entity<Agent>()
                .Property(a => a.SousCaisseAgent)
                .HasConversion<int>()
                .HasColumnType("NUMBER(1)");

            modelBuilder.Entity<Agent>()
                .Property(a => a.EstValide)
                .HasConversion<int>()
                .HasColumnType("NUMBER(1)");

            // Configure relationships
            modelBuilder.Entity<Agent>()
                .HasOne(a => a.Agence)
                .WithMany()
                .HasForeignKey(a => a.CodeAgence)
                .HasPrincipalKey(ag => ag.CodeAgence);

            modelBuilder.Entity<Agent>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.CodeService)
                .HasPrincipalKey(s => s.CodeService);

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