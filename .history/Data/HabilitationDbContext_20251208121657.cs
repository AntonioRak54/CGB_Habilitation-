using Microsoft.EntityFrameworkCore;

namespace CGB_Habilitation.Data
{
    public class HabilitationDbContext : DbContext
    {
        public HabilitationDbContext(DbContextOptions<HabilitationDbContext> options)
            : base(options)
        {
        }

        // Vous pouvez ajouter vos DbSet ici plus tard
        // public DbSet<Habilitation> Habilitations { get; set; }
    }
}