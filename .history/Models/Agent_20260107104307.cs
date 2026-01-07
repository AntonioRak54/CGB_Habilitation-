using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGB_Habilitation.Models
{
    public class Agent
    {
        public Guid IdAgent { get; set; }
        //Id on the db
        public string CodeAgent { get; set; }
        //Id utilisé dans le recherche 
        public string NomAgent { get; set; }
        public string PrenomAgent { get; set; }
        public string EmailAgent { get; set; }

        public string LoginAgent { get; set; }
        public string PasswordAgent { get; set; }

        public bool SousCaisseAgent { get; set; }
        public DateTime? DateLogin { get; set; }

        public bool EstValide { get; set; }
        public Guid IdRole { get; set; }
        public Role? RoleAgent { get; set; }

        public Guid CodeAgence { get; set; }
        public Agence? Agence { get; set; }

        public Guid CodeService { get; set; }
        public Service? Service { get; set; }

        public ICollection<Habilitation> Habilitations { get; set; } = new List<Habilitation>();
    }

}