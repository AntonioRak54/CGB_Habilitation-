using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGB_Habilitation.Models
{
    public class Habilitation
    {
        public Guid IdHabilitation { get; set; }

        public string TypeHabilitation { get; set; }
        public string Etat { get; set; }

        public DateTime DateCreationDemande { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        public string Motif { get; set; }

        public string PieceJoint { get; set; } 

        // Relations
        public Guid IdAgent { get; set; }
        public Agent Agent { get; set; }

        public Guid CreerPar { get; set; } 
        public Guid? TraiterPar { get; set; } 
    }

}