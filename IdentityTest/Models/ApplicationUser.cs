using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            ApplicationUserTijdvakken = new List<ApplicationUserTijdvak>();
        }
        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisatie")]
        public string Organisation { get; set; }

        // override it so we can localize the displaystring
        [Display(Name = "Telefoonnummer")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        public ICollection<ApplicationUserTijdvak> ApplicationUserTijdvakken { get; set; }

        [NotMapped]
        public bool isAdmin { get; set; }
        public async Task AddTijdvakkenAsync(ApplicationDbContext context)
        {
            var tvlist = await context.Tijdvakken.ToListAsync();
            foreach (Tijdvak tv in tvlist)
            {
                ApplicationUserTijdvakken.Add(new ApplicationUserTijdvak { TijdvakId = tv.Id, ApplicationUserId = Id });
            }
        }

    }
}