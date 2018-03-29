using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            ApplicationUserTijdvakken = new List<ApplicationUserTijdvak>();
            DateCreated = DateTime.UtcNow;
        }

        [Display(Name = "Datum")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM}")]
        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisatie")]
        public string Organisation { get; set; }

        [Display(Name="Divisie")]
        public string Department { get; set; }

        // override it so we can localize the displaystring
        [Display(Name = "Telefoonnummer")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        public bool isRDW { get; set; }

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

        public async static Task<int> ConfirmedUserCountAsync(ApplicationDbContext context)
        {
            var userCount = await context.Users.CountAsync(t => t.EmailConfirmed == true);
            return (int)userCount;
        }

        public async static Task<int> ConfirmedRdwUserCountAsync(ApplicationDbContext context)
        {
            var userCount = await context.Users
                .Where(t => t.isRDW == true)
                .CountAsync(t => t.EmailConfirmed == true);

            return (int)userCount;
        }

        public async static Task<int> ConfirmedNonRdwUserCountAsync(ApplicationDbContext context)
        {
            var userCount = await context.Users
                .Where(t => t.isRDW == false)
                .CountAsync(t => t.EmailConfirmed == true);

            return (int)userCount;
        }

        public async static Task<int> UnconfirmedUserCountAsync(ApplicationDbContext context)
        {
            var userCount = await context.Users.CountAsync(t => t.EmailConfirmed == false);
            return (int)userCount;
        }

        public async static Task<int> RdwUserCountAsync(ApplicationDbContext context)
        {
            var userCount = await context.Users.CountAsync(t => t.isRDW == true);
            return (int)userCount;

        }

        public async static Task<int> NonRdwuserCountAsync(ApplicationDbContext context)
        {
            var userCount = await context.Users.CountAsync(t => t.isRDW== false);
            return (int)userCount;
        }

        public async static Task<bool> HasReachedMaxRdwOrSiteLocked(ApplicationDbContext context)
        {
            Maxima maxima = await context.Maxima.FirstOrDefaultAsync();
            int count = await ConfirmedRdwUserCountAsync(context);

            return (count >= maxima.MaxRDW || maxima.SiteHasBeenLocked );
        }

        public async static Task<bool> HasReachedMaxNonRdwOrSiteLocked(ApplicationDbContext context)
        {
            Maxima maxima = await context.Maxima.FirstOrDefaultAsync();
            int count = await ConfirmedNonRdwUserCountAsync(context);

            return (count >= maxima.MaxNonRDW || maxima.SiteHasBeenLocked);
        }

    }
}