using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            Maxima maxima = await _context.Maxima.FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(User);
            if (_signInManager.IsSignedIn(User) && user == null)
            {
                // Workaround for a bug in the EF Identity framework, 
                // if an identity cookie was used before dropping the database and the you start the app (which creates a new database) without logging out before the drop, 
                // you get a situation where EF SignInManager thinks you are signed in, but the UserManager.GetUserAsync will return an empty string (as the cookie refers to a now deleted user). Which gives errors all over the place
                // I have tried several alternatives to fix it, but deleting the Identity cookie seems the only solution

                HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.SiteHasBeenLocked = maxima.SiteHasBeenLocked;
                return View();
            }
        }

        public IActionResult Privacy()
        {
            ViewData["Message"] = "";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}