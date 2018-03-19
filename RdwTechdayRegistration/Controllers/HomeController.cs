using RdwTechdayRegistration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RdwTechdayRegistration.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RdwTechdayRegistration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            Maxima maxima = await _context.Maxima.FirstOrDefaultAsync();
            ViewBag.SiteHasBeenLocked = maxima.SiteHasBeenLocked;

            return View();
        }

        public  IActionResult Privacy()
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