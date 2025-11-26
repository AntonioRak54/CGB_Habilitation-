using HabilitationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HabilitationApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UtilisateurRole> UtilisateurRoles { get; set; }
        public DbSet<Profil> Profils { get; set; }
        public DbSet<Habilitation> Habilitations { get; set; }
        // public DbSet<Notification> Notifications { get; set; }
        // public DbSet<AuditLog> AuditLogs { get; set; }
        // public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
        // public DbSet<SousCaisseRequest> SousCaisseRequests { get; set; }
        // public DbSet<HistoriqueEntry> HistoriqueEntries { get; set; }
        // public DbSet<Agence> Agences { get; set; }
        // public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Utilisateur
            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Login).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(200);
                entity.HasIndex(e => e.Login).IsUnique();
            });

            // Role
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId);

            // UtilisateurRole
            modelBuilder.Entity<UtilisateurRole>(ur =>
            {
                ur.HasKey(x => x.UtilisateurRoleId);
                ur.HasOne(x => x.Utilisateur).WithMany(u => u.UtilisateurRoles).HasForeignKey(x => x.UserId);
                ur.HasOne(x => x.Role).WithMany(r => r.UtilisateurRoles).HasForeignKey(x => x.RoleId);
            });

            // Habilitation
            modelBuilder.Entity<Habilitation>(h =>
            {
                h.HasKey(x => x.HabilitationId);
                h.HasOne(x => x.Utilisateur).WithMany(u => u.Habilitations).HasForeignKey(x => x.UserId);
                h.HasOne(x => x.Profil).WithMany().HasForeignKey(x => x.ProfilId);
            });

            // Autres tables -> conventions par défaut
        }
    }
}
