using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Models
{
    public class SessieTijdvak
    {
        public int SessieId { get; set; }
        public int TijdvakId { get; set; }
        public Sessie Sessie { get; set; }
        public Tijdvak Tijdvak { get; set; }
    }
}
