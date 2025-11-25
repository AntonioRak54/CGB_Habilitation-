using System;
using System.Collections.Generic;

namespace HabilitationApp.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string NomRole { get; set; }
        public string Description { get; set; }

        public ICollection<UtilisateurRole> UtilisateurRoles { get; set; } = new List<UtilisateurRole>();
    }
}

