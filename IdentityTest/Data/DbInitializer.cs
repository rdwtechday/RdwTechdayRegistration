using IdentityTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTest.Data
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
            _context.Database.EnsureCreated();
//            if ( _context.Database.GetPendingMigrations().Count() != 0 )
 //           {
  //              return;
   //         }


            // check if roles table populated, if not -> populate
            if (_roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Id }).ToList().Count == 0)
            {
                // insert basis roles
                await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            // check if basic admin account from Azure environment is present
            var userid = _configuration["adminuser:email"];
            var password = _configuration["adminuser:password"];
            var user = await _userManager.Users.SingleOrDefaultAsync<ApplicationUser>(u => u.Email == userid);
            if (user == null)
            {
                // add admin user
                var appuser = new ApplicationUser { UserName = userid, Email = userid, EmailConfirmed = true };
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

        }
    }
}