using RdwTechdayRegistration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SessiesController : Controller
    {
        private readonly RdwTechdayRegistration.Data.ApplicationDbContext _context;

        private void PopulateTracksDropDownList(object selectedTrack = null)
        {
            var query = from d in _context.Tracks    
                                   orderby d.Naam
                                   select d;
            ViewBag.TrackId = new SelectList(query.AsNoTracking(), "Id", "Naam", selectedTrack);
        }

        private void PopulateTijdvakDropDownList(object selectedTijdvak = null)
        {
            var query = from d in _context.Tijdvakken
                        orderby d.Order
                        select d;
            ViewBag.TijdvakId = new SelectList(query.AsNoTracking(), "Id", "Start", selectedTijdvak);
        }

        private void PopulateRuimtesDropDownList(object selectedRuimte = null)
        {
            var query = from d in _context.Ruimtes
                        orderby d.Naam
                        select d;
            ViewBag.RuimteId = new SelectList(query.AsNoTracking(), "Id", "Naam", selectedRuimte);
        }

        public SessiesController(RdwTechdayRegistration.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sessies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sessies.ToListAsync());
        }

        // GET: Sessies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessie = await _context.Sessies
                .Include(c => c.Ruimte)
                .Include(c => c.Tijdvak)
                .Include(c => c.Track)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (sessie == null)
            {
                return NotFound();
            }

            return View(sessie);
        }

        // GET: Sessies/Create
        public IActionResult Create()
        {
            PopulateTracksDropDownList();
            PopulateTijdvakDropDownList();
            PopulateRuimtesDropDownList();
            return View();
        }

        // POST: Sessies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,TrackId,TijdvakId,RuimteId")] Sessie sessie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sessie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateTracksDropDownList(sessie.TrackId);
            PopulateTijdvakDropDownList(sessie.TijdvakId);
            PopulateRuimtesDropDownList(sessie.RuimteId);
            return View(sessie);
        }

        // GET: Sessies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessie = await _context.Sessies
                .Include(c => c.Ruimte)
                .Include(c => c.Tijdvak)
                .Include(c => c.Track)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sessie == null)
            {
                return NotFound();
            }
            PopulateTracksDropDownList(sessie.TrackId);
            PopulateTijdvakDropDownList(sessie.TijdvakId);
            PopulateRuimtesDropDownList(sessie.RuimteId);
            return View(sessie);
        }

        // POST: Sessies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,TrackId,TijdvakId,RuimteId")] Sessie sessie)
        {
            if (id != sessie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sessie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessieExists(sessie.Id))
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
            PopulateTracksDropDownList(sessie.TrackId);
            PopulateTijdvakDropDownList(sessie.TijdvakId);
            PopulateRuimtesDropDownList(sessie.RuimteId);
            return View(sessie);
        }

        // GET: Sessies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessie = await _context.Sessies
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sessie == null)
            {
                return NotFound();
            }

            return View(sessie);
        }

        // POST: Sessies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessie = await _context.Sessies.SingleOrDefaultAsync(m => m.Id == id);
            _context.Sessies.Remove(sessie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessieExists(int id)
        {
            return _context.Sessies.Any(e => e.Id == id);
        }
    }
}