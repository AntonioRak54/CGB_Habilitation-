using System;
using CGB_Habilitation.Models;

namespace HabilitationApp.Models
{
    public class UtilisateurRole
    {
        public Guid UtilisateurRoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime DateAssign { get; set; } = DateTime.UtcNow;
        public DateTime? DateFin { get; set; }
        public bool EstActif { get; set; } = true;
        public required string Commentaire { get; set; }

        public required Utilisateur Utilisateur { get; set; }
        public required Role Role { get; set; }
    }
}
