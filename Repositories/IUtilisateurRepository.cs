using HabilitationApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabilitationApp.Repositories
{
    public interface IUtilisateurRepository : IGenericRepository<Utilisateur>
    {
        Task<Utilisateur?> GetByLoginAsync(string login);  // Made nullable
        Task<IEnumerable<Utilisateur>> GetWithRolesAsync();
    }
}