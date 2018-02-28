using RdwTechdayRegistration.Models;
using System.Collections.Generic;

namespace RdwTechdayRegistration.Models
{
    public class Deelnemer
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Organisatie { get; set; }
        public string Email { get; set; }
        public string Telefoon { get; set; }
    }
}