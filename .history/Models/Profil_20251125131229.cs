using System;

namespace HabilitationApp.Models
{
    public class Profil
    {
        public Guid ProfilId { get; set; }
        public required string NomProfil { get; set; }
        public required string CodeFonction { get; set; }
        public required string Description { get; set; }
    }
}
