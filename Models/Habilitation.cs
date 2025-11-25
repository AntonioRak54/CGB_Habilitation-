using System;

namespace HabilitationApp.Models
{
    public class Habilitation
    {
        public Guid HabilitationId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProfilId { get; set; }
        public required string Type { get; set; } 
        public DateTime DateDemande { get; set; } = DateTime.UtcNow;
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public required string Statut { get; set; } 
        public required string Motif { get; set; }
        public Guid? CreatePar { get; set; }
        public Guid? TraitePar { get; set; }

        public required Utilisateur Utilisateur { get; set; }
        public required Profil Profil { get; set; }
    }
}
