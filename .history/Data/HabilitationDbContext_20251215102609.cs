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
    public DbSet<Role> Roles { get; set; }
    public DbSet<Agence> Agences { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Habilitation> Habilitations { get; set; }
    }
}