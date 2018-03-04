using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RdwTechdayRegistration.Models
{
    public class Tijdvak
    {
        public int Id { get; set; }
        [Display(Name = "Volgorde")]
        [Required]
        public int Order { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string Einde { get; set; }
        public List<SessieTijdvak> SessieTijdvakken { get; set; }
        public List<ApplicationUserTijdvak> ApplicationUserTijdvakken { get; set; }
    }
}