using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTest.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;

namespace RdwTechdayRegistration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RuimtesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RuimtesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ruimtes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ruimtes.ToListAsync());
        }

        // GET: Ruimtes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruimte = await _context.Ruimtes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ruimte == null)
            {
                return NotFound();
            }

            return View(ruimte);
        }

        // GET: Ruimtes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ruimtes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,Capacity")] Ruimte ruimte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ruimte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ruimte);
        }

        // GET: Ruimtes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruimte = await _context.Ruimtes.SingleOrDefaultAsync(m => m.Id == id);
            if (ruimte == null)
            {
                return NotFound();
            }
            return View(ruimte);
        }

        // POST: Ruimtes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,Capacity")] Ruimte ruimte)
        {
            if (id != ruimte.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ruimte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RuimteExists(ruimte.Id))
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
            return View(ruimte);
        }

        // GET: Ruimtes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruimte = await _context.Ruimtes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ruimte == null)
            {
                return NotFound();
            }

            return View(ruimte);
        }

        // POST: Ruimtes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ruimte = await _context.Ruimtes.SingleOrDefaultAsync(m => m.Id == id);
            _context.Ruimtes.Remove(ruimte);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RuimteExists(int id)
        {
            return _context.Ruimtes.Any(e => e.Id == id);
        }
    }
}
