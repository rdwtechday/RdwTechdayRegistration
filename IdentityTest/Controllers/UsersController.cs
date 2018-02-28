using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RdwTechdayRegistration.Controllers
{
    public class UsersController : Controller
    {
        private readonly RdwTechdayRegistration.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(RdwTechdayRegistration.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Deelnemers
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers.ToListAsync());
        }

        // GET: Deelnemers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deelnemer = await _context.ApplicationUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (deelnemer == null)
            {
                return NotFound();
            }

            return View(deelnemer);
        }

        // GET: Deelnemers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deelnemers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,Organisatie,Email,Telefoon")] ApplicationUser deelnemer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deelnemer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deelnemer);
        }

        // GET: Deelnemers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deelnemer = await _context.ApplicationUsers.SingleOrDefaultAsync(m => m.Id == id);
            if (deelnemer == null)
            {
                return NotFound();
            }
            return View(deelnemer);
        }

        // POST: Deelnemers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Organisation,Email,PhoneNumber")] ApplicationUser deelnemer)
        {
            if (id != deelnemer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(deelnemer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deelnemer);
        }

        // GET: Deelnemers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deelnemer = await _context.ApplicationUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (deelnemer == null)
            {
                return NotFound();
            }

            return View(deelnemer);
        }

        // POST: Deelnemers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var deelnemer = await _context.ApplicationUsers.SingleOrDefaultAsync(m => m.Id == id);
            await _userManager.DeleteAsync(deelnemer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}