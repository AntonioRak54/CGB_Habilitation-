using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGB_Habilitation.Models
{
    public class Service
    {
        public Guid CodeService { get; set; }
        public string NomService { get; set; }
        public string TypeService { get; set; } // BO / FO
    }

}