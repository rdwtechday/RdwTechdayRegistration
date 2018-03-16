using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RdwTechdayRegistration.Models
{
    public class Track
    {
        public int Id { get; set; }
        [Display(Name = "Track")]
        public string Naam { get; set; }

        /// <summary>
        /// The RGB colorcode to show in the screen
        /// </summary>
        [Display(Name = "RGB Kleurcode")]
        public string Rgb { get; set; }

        [Display(Name = "Badge Code")]
        public string BadgeCode { get; set; }

        public List<Sessie> Sessies { get; set; }
        //public List<ApplicationUserTijdvak> ApplicationUserTijdvakken { get; set; }
        //public List<TrackTijdvak> Tijdvakken { get; set; }
    }
}