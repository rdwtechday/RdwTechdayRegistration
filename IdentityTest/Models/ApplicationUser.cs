/*  Copyright (C) 2018, RDW 
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Affero General Public License for more details.
 *
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/.  
 *  
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Data;
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
        }

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

        public async static Task<bool> HasReachedMaxRdw(ApplicationDbContext context)
        {
            Maxima maxima = await context.Maxima.FirstOrDefaultAsync();
            int count = await ConfirmedRdwUserCountAsync(context);

            return (count >= maxima.MaxRDW);
        }

        public async static Task<bool> HasReachedMaxNonRdw(ApplicationDbContext context)
        {
            Maxima maxima = await context.Maxima.FirstOrDefaultAsync();
            int count = await ConfirmedNonRdwUserCountAsync(context);

            return (count >= maxima.MaxNonRDW);
        }

    }
}