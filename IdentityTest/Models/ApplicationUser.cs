using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RdwTechdayRegistration.ValidationHelpers;

namespace IdentityTest.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name{ get; set; }
        [Required]
        public string Organisation { get; set; }
    }
}
