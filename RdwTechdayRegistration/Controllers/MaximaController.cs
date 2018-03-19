using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RdwTechdayRegistration.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MaximaController : Controller
    {
        private readonly RdwTechdayRegistration.Data.ApplicationDbContext _context;
        private IConfiguration _configuration;

        public MaximaController(RdwTechdayRegistration.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Maxima
        public async Task<IActionResult> Index()
        {
            var maxima = await _context.Maxima.SingleOrDefaultAsync();
            if (maxima == null)
            {
                // if maxima table empty create placeholder record
                maxima = new Maxima { MaxRDW = 0, MaxNonRDW = 0 };
                _context.Maxima.Add(maxima);
                await _context.SaveChangesAsync();
            }
            int confirmedUserCount = await ApplicationUser.ConfirmedUserCountAsync(_context);
            int unconfirmedUserCount = await ApplicationUser.UnconfirmedUserCountAsync(_context);
            int confirmedRdwUserCount = await ApplicationUser.ConfirmedRdwUserCountAsync(_context);
            int confirmedNonRdwUserCount = await ApplicationUser.ConfirmedNonRdwUserCountAsync(_context);
            int rdwUserCount = await ApplicationUser.RdwUserCountAsync(_context);
            int nonRdwUserCount = await ApplicationUser.NonRdwuserCountAsync(_context);

            ViewBag.ConfirmedUserCount = confirmedUserCount.ToString();
            ViewBag.UnconfirmedUserCount = unconfirmedUserCount.ToString();
            ViewBag.ConfirmedRdwUserCount = confirmedRdwUserCount.ToString();
            ViewBag.ConfirmedNonRdwUserCount = confirmedNonRdwUserCount.ToString();
            ViewBag.UnconfirmedRdwUserCount = rdwUserCount - confirmedRdwUserCount;
            ViewBag.UnconfirmedNonRdwUserCount = nonRdwUserCount - confirmedNonRdwUserCount;

            List<ApplicationUser> users = await _context.ApplicationUsers
                .Include(t => t.ApplicationUserTijdvakken)
                /* this is to filter older users that did not have a datecreated field as they where created before adding the field */
                .Where(t => t.DateCreated > new System.DateTime(2017, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc))
                .OrderByDescending(t => t.DateCreated)
                .Take(20)
                .ToListAsync();

            maxima.LastCreatedUsers = users;

            return View(maxima);
        }

        // POST: Maxima/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaxRDW,MaxNonRDW,SiteHasBeenLocked")] Maxima maxima)
        {
            if (id != maxima.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var dbmaxima = await _context.Maxima.SingleOrDefaultAsync(t => t.Id == maxima.Id);
                try
                {
                    dbmaxima.MaxRDW = maxima.MaxRDW;
                    dbmaxima.MaxNonRDW = maxima.MaxNonRDW;
                    dbmaxima.SiteHasBeenLocked = maxima.SiteHasBeenLocked;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaximaExists(maxima.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View(maxima);
        }

        private bool MaximaExists(int id)
        {
            return _context.Maxima.Any(e => e.Id == id);
        }
    }
}