using System;
using System.Collections.Generic;

namespace HabilitationApp.Models
{
    public class Utilisateur
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DernierLogin { get; set; }
        public bool EstActif { get; set; } = true;

        public ICollection<UtilisateurRole> UtilisateurRoles { get; set; } = new List<UtilisateurRole>();
        public ICollection<Habilitation> Habilitations { get; set; } = new List<Habilitation>();
    }
}
