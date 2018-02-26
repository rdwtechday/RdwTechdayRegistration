using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;

namespace RdwTechdayRegistration.Controllers
{
    public class DeelnemersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeelnemersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Deelnemers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Deelnemers.ToListAsync());
        }

        // GET: Deelnemers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deelnemer = await _context.Deelnemers
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
        public async Task<IActionResult> Create([Bind("Id,Naam,Organisatie,Email,Telefoon")] Deelnemer deelnemer)
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deelnemer = await _context.Deelnemers.SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,Organisatie,Email,Telefoon")] Deelnemer deelnemer)
        {
            if (id != deelnemer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deelnemer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeelnemerExists(deelnemer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(deelnemer);
        }

        // GET: Deelnemers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deelnemer = await _context.Deelnemers
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deelnemer = await _context.Deelnemers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Deelnemers.Remove(deelnemer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeelnemerExists(int id)
        {
            return _context.Deelnemers.Any(e => e.Id == id);
        }
    }
}
