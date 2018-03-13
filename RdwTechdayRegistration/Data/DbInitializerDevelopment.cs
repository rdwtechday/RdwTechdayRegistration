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
    public class DbInitializerDevelopement : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Program> _logger;

        public DbInitializerDevelopement(
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
            //return;
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

            var faker = new Faker("nl");
            Randomizer.Seed = new System.Random(8675309);

            var tracks = new List<Track>();
            var sessies = new List<Sessie>();

            var track0 = new Track { Naam = "Keynote", Rgb = "B9CA8A" };
            tracks.Add(track0);
            var sessie = new Sessie { Naam = "Een mooie keynote lezing", Ruimte = room250, Track = track0 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv0 });


            var track1 = new Track { Naam = "Infra", Rgb = "FFC548" }; tracks.Add(track1);
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomGrn, Track = track1 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv1 } );
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomGrn, Track = track1 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv2 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomGrn, Track = track1 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv3 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomGrn, Track = track1 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv4 });

            var track2 = new Track { Naam = "Apps", Rgb = "B0A0C5" }; tracks.Add(track2);
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomAms, Track = track2 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie= sessie, Tijdvak = tv1 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomAms, Track = track2 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv2 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomAms, Track = track2 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv3 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomAms, Track = track2 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv4 });

            var track3 = new Track { Naam = "Collaboratie", Rgb = "9CD2E0" }; tracks.Add(track3);
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomRdm, Track = track3 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv1 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomRdm, Track = track3 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv2 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomRdm, Track = track3 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv3 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomRdm, Track = track3 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv4 });

            var track4 = new Track { Naam = "Inspiratie", Rgb = "FFAC59" }; tracks.Add(track4);
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomUtr, Track = track4 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv1 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomUtr, Track = track4 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv2 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomUtr, Track = track4 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv3 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomUtr, Track = track4 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv4 });

            var track5 = new Track { Naam = "Workshops I", Rgb = "D99690" }; tracks.Add(track5);
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomLei, Track = track5 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv1 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomLei, Track = track5 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv2 });
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv3 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomLei, Track = track5 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv4 });

            var track6 = new Track { Naam = "Workshops II", Rgb = "D99690" }; tracks.Add(track6);
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomDel, Track = track6 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv1 });
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv2 });
            sessie = new Sessie { Naam = faker.Lorem.Sentence(7), Ruimte = roomDel, Track = track6 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv3 });
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv4 });

            var track7 = new Track { Naam = "Closing Note", Rgb = "B9CA8A" }; tracks.Add(track7);
            sessie = new Sessie { Naam = "Een mooie closing note lezing", Ruimte = room250, Track = track7 }; sessies.Add(sessie);
            sessie.SessieTijdvakken.Add(new SessieTijdvak { Sessie = sessie, Tijdvak = tv5 });

            _context.Tracks.AddRange(tracks);
            _context.Sessies.AddRange(sessies);

            var sessieresult = await _context.SaveChangesAsync();
        }
    }
}