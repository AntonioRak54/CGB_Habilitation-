using System;

namespace HabilitationApp.Models
{
    public class Habilitation
    {
        public Guid HabilitationId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProfilId { get; set; }
        public string Type { get; set; } 
        public DateTime DateDemande { get; set; } = DateTime.UtcNow;
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public string Statut { get; set; } 
        public string Motif { get; set; }
        public Guid? CreatePar { get; set; }
        public Guid? TraitePar { get; set; }

        public Utilisateur Utilisateur { get; set; }
        public Profil Profil { get; set; }
    }
}
