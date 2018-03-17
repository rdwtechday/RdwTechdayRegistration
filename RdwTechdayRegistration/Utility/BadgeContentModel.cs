using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Utility
{

    public enum BadgePersonType {
        [Display(Name = "Normale Deelnemer")]
        user,
        [Display(Name = "Techday Organisatie")]
        organizer,
        [Display(Name = "Spreker")]
        speaker
    }

    public class BadgeContentModel
    {
        [Display(Name = "Voor- en Achternaam")]
        public string name { get; set; }
        [Display(Name = "Organisatie (en evt. afdeling)")]
        public string organisation { get; set; }
        [Display(Name = "Type persoon")]
        public BadgePersonType PersonType { get; set; }
    }
}
