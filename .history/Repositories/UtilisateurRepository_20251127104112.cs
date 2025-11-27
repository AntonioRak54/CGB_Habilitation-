using HabilitationApp.Data;
using HabilitationApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabilitationApp.Repositories
{
    public class UtilisateurRepository : GenericRepository<Utilisateur>, IUtilisateurRepository
    {
        public UtilisateurRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Utilisateur> GetByLoginAsync(string login)
        {
            return await _ctx.Set<Utilisateur>()
                .FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<IEnumerable<Utilisateur>> GetWithRolesAsync()
        {
            // If you have a Roles navigation property, include it:
            return await _ctx.Set<Utilisateur>()
                // .Include(u => u.Roles) // Uncomment when you have roles
                // .Include(u => u.Habilitation) // Uncomment when you have habilitation
                .AsNoTracking()
                .ToListAsync();
        }
    }

    internal interface IUtilisateurRepository
    {
    }
}