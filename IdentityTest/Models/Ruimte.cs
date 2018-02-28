using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RdwTechdayRegistration.Models
{
    public class Ruimte
    {
        public int Id { get; set; }
        [Display(Name = "Ruimte")]
        public string Naam { get; set; }
        public int Capacity { get; set; }
        public List<Sessie> Sessies { get; set; }
    }
}