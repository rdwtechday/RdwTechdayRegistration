using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace RdwTechdayRegistration.Controllers

{
    [Authorize(Roles = "Admin")]

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