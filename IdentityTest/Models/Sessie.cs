using RdwTechdayRegistration.Models;
using System.Collections.Generic;

namespace RdwTechdayRegistration.Models
{
    public class Sessie
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public ICollection<ApplicationUserSessie> UserSessies { get; set; }
        public int RuimteId { get; set; }
        public Ruimte Ruimte { get; set; }
        public int TijdvakId { get; set; }
        public Tijdvak Tijdvak { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
    }
}