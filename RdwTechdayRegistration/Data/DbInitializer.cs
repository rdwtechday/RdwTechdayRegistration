using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RdwTechdayRegistration.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Program> _logger;

        public DbInitializer(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<Program> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Initialize()
        {
            await _context.Database.MigrateAsync();
            if (_context.Maxima.Any())
            {
                return;   // DB has been seeded
            }

            var maxima = new List<Maxima>();
            maxima.Add(new Maxima { MaxRDW = 200, MaxNonRDW = 50 });
            _context.Maxima.AddRange(maxima);

            var tijdvakken = new List<Tijdvak>();
            var tv0 = new Tijdvak { Start = "09:00", Einde = "09:45", Order = 1 };
            tijdvakken.Add(tv0);
            var tv1 = new Tijdvak { Start = "10:00", Einde = "11:00", Order = 2 };
            tijdvakken.Add(tv1);
            var tv2 = new Tijdvak { Start = "11:15", Einde = "12:15", Order = 3 };
            tijdvakken.Add(tv2);
            var tv3 = new Tijdvak { Start = "13:15", Einde = "14:15", Order = 4 };
            tijdvakken.Add(tv3);
            var tv4 = new Tijdvak { Start = "14:30", Einde = "15:30", Order = 5 };
            tijdvakken.Add(tv4);
            var tv5 = new Tijdvak { Start = "15:45", Einde = "16:30", Order = 6 };
            tijdvakken.Add(tv5);

            _context.Tijdvakken.AddRange(tijdvakken);

            // check if roles table populated, if not -> populate
            if (_roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Id }).ToList().Count == 0)
            {
                // insert basis roles
                await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            // check if basic admin account from Azure environment is present, if not create it and give or restore admin rights
            var userid = _configuration["adminuser:email"];
            var password = _configuration["adminuser:password"];
            var user = await _userManager.Users.SingleOrDefaultAsync<ApplicationUser>(u => u.Email == userid);
            if (user == null)
            {
                ApplicationUser appuser = new ApplicationUser { UserName = userid, Email = userid, EmailConfirmed = true, Name = userid, Organisation = "RdwTechday" };
                // add admin user
                await appuser.AddTijdvakkenAsync(_context);
 
                IdentityResult result = await _userManager.CreateAsync(appuser, password);
                if (!result.Succeeded)
                {
                    string msg = "";
                    foreach (IdentityError err in result.Errors) { msg = msg + err.Description; }
                    throw new System.InvalidOperationException("Error creating standard admin user" + msg);
                }
                user = await _userManager.Users.SingleOrDefaultAsync<ApplicationUser>(u => u.Email == userid);
                await _userManager.AddToRoleAsync(user, "User");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Admin"))
            {
                IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "Admin");
                if (!roleResult.Succeeded)
                {
                    string msg = "";
                    foreach (IdentityError err in roleResult.Errors) { msg = msg + err.Description; }
                    throw new System.InvalidOperationException("Error creating standard admin user" + msg);
                }
            }
            await _context.SaveChangesAsync();

            var ruimtes = new List<Ruimte>();
            var room250 = new Ruimte { Naam = "Grote zaal", Capacity = 250 };
            ruimtes.Add(room250);
            var roomGrn = new Ruimte { Naam = "Groningen", Capacity = 70 };
            ruimtes.Add(roomGrn);
            var roomAms = new Ruimte { Naam = "Amsterdam", Capacity = 70 };
            ruimtes.Add(roomAms);
            var roomRdm = new Ruimte { Naam = "Rotterdam", Capacity = 70 };
            ruimtes.Add(roomRdm);
            var roomUtr = new Ruimte { Naam = "Utrecht", Capacity = 70 };
            ruimtes.Add(roomUtr);
            var roomNij = new Ruimte { Naam = "Nijmegen", Capacity = 22 };
            ruimtes.Add(roomNij);
            var roomLei = new Ruimte { Naam = "Leiden", Capacity = 22 };
            ruimtes.Add(roomLei);
            var roomDel = new Ruimte { Naam = "Delft", Capacity = 22 };
            ruimtes.Add(roomDel);
            _context.Ruimtes.AddRange(ruimtes);


            var tracks = new List<Track>();

            var track0 = new Track { Naam = "Keynote", Rgb = "B9CA8A", BadgeCode = "KN" }; tracks.Add(track0);
            var track1 = new Track { Naam = "Infra", Rgb = "FFC548", BadgeCode= "IF" }; tracks.Add(track1);
            var track2 = new Track { Naam = "Apps", Rgb = "B0A0C5", BadgeCode = "AP" }; tracks.Add(track2);
            var track3 = new Track { Naam = "Collaboratie", Rgb = "9CD2E0", BadgeCode = "CO" }; tracks.Add(track3);
            var track4 = new Track { Naam = "Inspiratie", Rgb = "FFAC59", BadgeCode = "IS" }; tracks.Add(track4);
            var track5 = new Track { Naam = "Workshops I", Rgb = "D99690", BadgeCode = "W1" }; tracks.Add(track5);
            var track6 = new Track { Naam = "Workshops II", Rgb = "D99690", BadgeCode = "W2" }; tracks.Add(track6);
            var track7 = new Track { Naam = "Closing Note", Rgb = "B9CA8A", BadgeCode = "CN" }; tracks.Add(track7);

            _context.Tracks.AddRange(tracks);

            var initresult = await _context.SaveChangesAsync();
        }
    }
}