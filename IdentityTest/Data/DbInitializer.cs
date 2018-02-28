using RdwTechdayRegistration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using System.Collections.Generic;

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
                // add admin user
                var appuser = new ApplicationUser { UserName = userid, Email = userid, EmailConfirmed = true, Name=userid, Organisation = "RdwTechday" };
                IdentityResult result = await _userManager.CreateAsync(appuser, password);
                if (!result.Succeeded)
                {
                    string msg = "";
                    foreach (IdentityError err in result.Errors) { msg = msg + err.Description; }
                    throw new System.InvalidOperationException("Error creating standard admin user" + msg);
                }
                user = await _userManager.Users.SingleOrDefaultAsync<ApplicationUser>(u => u.Email == userid);
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

            if (_context.Maxima.Any())
            {
                return;   // DB has been seeded
            }

            var maxima = new List<Maxima>();
            maxima.Add(new Maxima { MaxRDW = 200, MaxNonRDW = 50 });
            _context.Maxima.AddRange(maxima);

            var ruimtes = new List<Ruimte>();
            ruimtes.Add(new Ruimte { Naam = "Groningen", Capacity = 70 });
            ruimtes.Add(new Ruimte { Naam = "Amsterdam", Capacity = 70 });
            ruimtes.Add(new Ruimte { Naam = "Rotterdam", Capacity = 70 });
            ruimtes.Add(new Ruimte { Naam = "Utrecht", Capacity = 70 });
            ruimtes.Add(new Ruimte { Naam = "Nijmegen", Capacity = 22 });
            ruimtes.Add(new Ruimte { Naam = "Leiden", Capacity = 22 });
            ruimtes.Add(new Ruimte { Naam = "Delft", Capacity = 22 });
            _context.Ruimtes.AddRange(ruimtes);

            var tijdvakken = new List<Tijdvak>();
            var tv11 = new Tijdvak { Start = "10:00", Einde = "11:00", Order = 1 };
            tijdvakken.Add(tv11);
            var tv12 = new Tijdvak { Start = "11:15", Einde = "12:15", Order = 2 };
            tijdvakken.Add(tv12);
            var tv13 = new Tijdvak { Start = "13:15", Einde = "14:15", Order = 3 };
            tijdvakken.Add(tv13);
            var tv14 = new Tijdvak { Start = "14:30", Einde = "15:30", Order = 4 };
            tijdvakken.Add(tv14);
            var tv21 = new Tijdvak { Start = "10:00", Einde = "12:15", Order = 1 };
            tijdvakken.Add(tv21);
            var tv22 = new Tijdvak { Start = "13:15", Einde = "15:30", Order = 2 };
            tijdvakken.Add(tv22);
            _context.Tijdvakken.AddRange(tijdvakken);

            var tracks = new List<Track>();
            tracks.Add(new Track { Naam = "Infra", Rgb = "FFC548" });
            tracks.Add(new Track { Naam = "Apps", Rgb = "B0A0C5" });
            tracks.Add(new Track { Naam = "Collaboratie", Rgb = "9CD2E0" });
            tracks.Add(new Track { Naam = "Inspiratie", Rgb = "FFAC59" });
            tracks.Add(new Track { Naam = "Workshops I", Rgb = "D99690" });
            tracks.Add(new Track { Naam = "Workshops II", Rgb = "D99690" });
            _context.Tracks.AddRange(tracks);

            await _context.SaveChangesAsync();

            // Creeer TRack met bijbehorende tijdvakken, workshop duren 2 uur, daarom aparte loop voor de workshops
            List<string> lecturenames = new List<string>() { "Infra", "Apps", "Inspiratie", "Collaboratie" };
            foreach (string naam in lecturenames)
            {
                _context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv11.Id });
                _context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv12.Id });
                _context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv13.Id });
                _context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv14.Id });
            }
            List<string> workshopnames = new List<string>() { "Workshops I", "Workshops II" };
            foreach (string naam in workshopnames)
            {
                _context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv21.Id });
                _context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv22.Id });
            }

            await _context.SaveChangesAsync();


            var faker = new Faker("nl");
            Randomizer.Seed = new System.Random(8675309);

            int maxDeelnemers = 50;
            int maxRegistratieVerzoeken = maxDeelnemers + 20;
            int maxDeelnemersMetSessies = maxDeelnemers - 20;

            // creeer sessies, itereer over Tracks en de Tijdvakken in een Track heen
            var sessies = new List<Sessie>();
            foreach (string naam in lecturenames)
            {
                Track track = tracks.Single(i => i.Naam == naam);
                foreach (TrackTijdvak tracktijdvak in track.Tijdvakken)
                {
                    sessies.Add(new Sessie { Naam = faker.Lorem.Sentence(3), Ruimte = faker.PickRandom<Ruimte>(ruimtes), Track = track, Tijdvak = tracktijdvak.Tijdvak });
                }
            }

            foreach (string naam in workshopnames)
            {
                Track track = tracks.Single(i => i.Naam == naam);
                foreach (TrackTijdvak tracktijdvak in track.Tijdvakken)
                {
                    sessies.Add(new Sessie { Naam = faker.Lorem.Sentence(3), Ruimte = faker.PickRandom<Ruimte>(ruimtes), Track = track, Tijdvak = tracktijdvak.Tijdvak });
                }
            }
            _context.Sessies.AddRange(sessies);

            await _context.SaveChangesAsync();




        }
    }
}