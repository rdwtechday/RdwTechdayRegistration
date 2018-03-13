using RdwTechdayRegistration.ValidationHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Models.ManageViewModels
{
    public class IndexViewModel 
    {

        [Display(Name= "Login")]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public bool isRdw { get; set; }

        [Required]
        [Display(Name = "Voor- en Achternaam")]
        public string Name{ get; set; }

        [Phone]
        [Display(Name = "Telefoonnummer")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Divisie")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Organisatie")]
        public string Organisation { get; set; }

        public string StatusMessage { get; set; }

    }
}
