using System;

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
        public string Commentaire { get; set; }

        public Utilisateur Utilisateur { get; set; }
        public Role Role { get; set; }
    }
}
