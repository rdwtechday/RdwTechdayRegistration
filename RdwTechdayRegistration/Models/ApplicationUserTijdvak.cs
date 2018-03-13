using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Models
{
    public class ApplicationUserTijdvak
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int TijdvakId { get; set; }
        public Tijdvak Tijdvak { get; set; }
        public int? SessieId { get; set; }
        public Sessie Sessie { get; set; }
    }
}
