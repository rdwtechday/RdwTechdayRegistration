using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RdwTechdayRegistration.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisatie")]
        public string Organisation { get; set; }

        // override it so we can localize the displaystring
        [Display(Name = "Telefoonnummer")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        public ICollection<ApplicationUserSessie> UserSessies { get; set; }

        [NotMapped]
        public bool isAdmin { get; set; }
    }
}