using System;
using System.Collections.Generic;

namespace HabilitationApp.Models
{
    public class Utilisateur
    {
        public Guid UserId { get; set; }
        public required string Login { get; set; }
        public required string Email { get; set; }
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public required string Telephone { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DernierLogin { get; set; }
        public bool EstActif { get; set; } = true;

        public ICollection<UtilisateurRole> UtilisateurRoles { get; set; } = new List<UtilisateurRole>();
        public ICollection<Habilitation> Habilitations { get; set; } = new List<Habilitation>();
    }
}
