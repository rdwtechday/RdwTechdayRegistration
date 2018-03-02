using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            List<ApplicationUser> users = await _context.ApplicationUsers.ToListAsync();
            // brute force, maybe refactor if too slow
            foreach (ApplicationUser user in users)
            {
                user.isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            }
            return View(users);
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

        [HttpGet]
        public async Task<IActionResult> AddAdminRole(string id)
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


        [HttpPost, ActionName("AddAdminRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdminRoleConfirmed(string id)
        {
            ApplicationUser deelnemer = await _context.ApplicationUsers.SingleOrDefaultAsync(m => m.Id == id);

            if ( !( await _userManager.IsInRoleAsync(deelnemer, "Admin") ) )
            {
                await _userManager.AddToRoleAsync(deelnemer, "Admin");
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RevokeAdminRole(string id)
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

        [HttpPost, ActionName("RevokeAdminRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RevokeAdminRoleConfirmed(string id)
        {
            var deelnemer = await _context.ApplicationUsers.SingleOrDefaultAsync(m => m.Id == id);
            if ( await _userManager.IsInRoleAsync(deelnemer, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(deelnemer, "Admin");
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}