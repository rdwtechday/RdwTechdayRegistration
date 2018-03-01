using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RdwTechdayRegistration.Models
{
    public class Tijdvak
    {
        public int Id { get; set; }
        [Display(Name = "Volgorde")]
        public int Order { get; set; }
        public string Start { get; set; }
        public string Einde { get; set; }
        public List<Sessie> Sessies { get; set; }

    }
}