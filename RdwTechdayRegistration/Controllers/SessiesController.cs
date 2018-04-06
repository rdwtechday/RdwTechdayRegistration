using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RdwTechdayRegistration.Models;
using RdwTechdayRegistration.Models.SessieViewModels;
using RdwTechdayRegistration.Utility;
using System.Collections.Generic;
using System.IO;
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

        private FileStreamResult ExcelResult(List<Sessie> sessies, Dictionary<int, int> userCounts)
        {
            NpoiMemoryStream stream = new NpoiMemoryStream();
            stream.AllowClose = false;
            IWorkbook workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet("Sessies");

            int iRow = 0;
            IRow row = excelSheet.CreateRow(iRow);
            row.CreateCell(0).SetCellValue("Naam");
            row.CreateCell(1).SetCellValue("Ruimte");
            row.CreateCell(2).SetCellValue("Track");
            row.CreateCell(3).SetCellValue("Tijd");
            row.CreateCell(4).SetCellValue("Deelnemers");
            row.CreateCell(5).SetCellValue("Capaciteit");

            foreach (Sessie sessie in sessies)
            {
                iRow++;
                row = excelSheet.CreateRow(iRow);
                row.CreateCell(0).SetCellValue(sessie.Naam);
                row.CreateCell(1).SetCellValue(sessie.Ruimte.Naam);
                row.CreateCell(2).SetCellValue(sessie.Track.Naam);
                row.CreateCell(3).SetCellValue(sessie.TimeRange());
                int count = 0;
                if ( userCounts.ContainsKey(sessie.Id) ) {
                    count = userCounts[sessie.Id];
                }
                row.CreateCell(4).SetCellValue(count);
                row.CreateCell(5).SetCellValue(sessie.Ruimte.Capacity);
            }
            workbook.Write(stream);
            stream.AllowClose = true;
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sessies.xlsx");
        }

        public async Task<IActionResult> Excel()
        {
            List<Sessie> sessies = await _context.Sessies
                .AsNoTracking()
                .Include(c => c.Ruimte)
                .Include(c => c.SessieTijdvakken)
                    .ThenInclude(stv => stv.Tijdvak)
                .Include(c => c.Track)
                .OrderBy(s => s.Naam)
                .ToListAsync();

            Dictionary<int, int> UserCounts = await Sessie.GetUserCountsAsync(_context);


            return ExcelResult(sessies, UserCounts);
        }

        public async Task<IActionResult> Index()
        {
            List<Sessie> sessies = await _context.Sessies
                .AsNoTracking()
                .Include(c => c.Ruimte)
                .Include(c => c.SessieTijdvakken)
                    .ThenInclude(stv => stv.Tijdvak)
                .Include(c => c.Track)
                .OrderBy(s => s.Naam)
                .ToListAsync();

            ViewBag.UserCounts = await Sessie.GetUserCountsAsync(_context);

            return View(sessies);
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
                .Include(c => c.SessieTijdvakken)
                    .ThenInclude(stv => stv.Tijdvak)
                .Include(c => c.Track)
                .Include(c => c.ApplicationUserTijdvakken)
                    .ThenInclude(c => c.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (sessie == null)
            {
                return NotFound();
            }

            /* bit brute force, but given low number of users it should be ok */
            var userIDs = new HashSet<string>();
            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (ApplicationUserTijdvak atv in sessie.ApplicationUserTijdvakken)
            {
                if (!userIDs.Contains(atv.ApplicationUserId))
                {
                    users.Add(atv.ApplicationUser);
                    userIDs.Add(atv.ApplicationUserId);
                }
            }
            ViewBag.Attendants = users.OrderBy(c => c.Name).ToList();
            return View(sessie);
        }

        // GET: Sessies/Create
        public IActionResult Create()
        {
            PopulateTracksDropDownList();
            PopulateRuimtesDropDownList();
            PopulateSessieTijdvakData();
            return View();
        }

        // POST: Sessies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Naam,TrackId,RuimteId")] Sessie sessie, string[] selectedTijdvakken)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sessie);
                UpdateSessieTijdvakken(selectedTijdvakken, sessie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateTracksDropDownList(sessie.TrackId);
            PopulateRuimtesDropDownList(sessie.RuimteId);
            PopulateSessieTijdvakData(sessie);
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
                .Include(c => c.SessieTijdvakken)
                    .ThenInclude(stv => stv.Tijdvak)
                .Include(c => c.Track)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sessie == null)
            {
                return NotFound();
            }
            PopulateTracksDropDownList(sessie.TrackId);
            PopulateRuimtesDropDownList(sessie.RuimteId);
            PopulateSessieTijdvakData(sessie);
            return View(sessie);
        }

        private void PopulateSessieTijdvakData()
        {
            var tijdvakken = _context.Tijdvakken.OrderBy(t => t.Order);

            var viewModel = new List<SessieTijdvakData>();
            foreach (var tv in tijdvakken)
            {
                viewModel.Add(new SessieTijdvakData
                {
                    TijdvakId = tv.Id,
                    Title = tv.TimeRange(),
                    Assigned = false
                });
            }
            ViewData["Tijdvakken"] = viewModel;
        }
        private void PopulateSessieTijdvakData(Sessie sessie)
        {
            var tijdvakken = _context.Tijdvakken.OrderBy(t => t.Order);

            var sessieTijdvakken = new HashSet<int>(sessie.SessieTijdvakken.Select(c => c.TijdvakId));
            var viewModel = new List<SessieTijdvakData>();
            foreach (var tv in tijdvakken)
            {
                viewModel.Add(new SessieTijdvakData
                {
                    TijdvakId = tv.Id,
                    Title = tv.TimeRange(),
                    Assigned = sessieTijdvakken.Contains(tv.Id)
                });
            }
            ViewData["Tijdvakken"] = viewModel;
        }

        private void PopulateSessieTijdvakData(Sessie sessie, string[] selectedTijdvakken)
        {
            var tijdvakken = _context.Tijdvakken.OrderBy(t => t.Order);

            var selectedTijdvakkenHS = new HashSet<string>(selectedTijdvakken);
            var viewModel = new List<SessieTijdvakData>();
            foreach (var tv in tijdvakken)
            {
                viewModel.Add(new SessieTijdvakData
                {
                    TijdvakId = tv.Id,
                    Title = tv.TimeRange(),
                    Assigned = selectedTijdvakkenHS.Contains(tv.Id.ToString())
                });
            }
            ViewData["Tijdvakken"] = viewModel;
        }

        // POST: Sessies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,TrackId,RuimteId")] Sessie sessie, string[] selectedTijdvakken)
        {
            if (id != sessie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Sessie updatedSessie = await _context.Sessies
                        .Include(s => s.SessieTijdvakken)
                            .ThenInclude(s => s.Tijdvak)
                        .SingleOrDefaultAsync(s => s.Id == id);

                    updatedSessie.Naam = sessie.Naam;
                    updatedSessie.RuimteId = sessie.RuimteId;
                    updatedSessie.TrackId = sessie.TrackId;

                    UpdateSessieTijdvakken(selectedTijdvakken, updatedSessie);

                    _context.Update(updatedSessie);
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
            PopulateSessieTijdvakData(sessie, selectedTijdvakken);
            PopulateTracksDropDownList(sessie.TrackId);
            PopulateRuimtesDropDownList(sessie.RuimteId);
            return View(sessie);
        }

        private void UpdateSessieTijdvakken(string[] selectedTijdvakken, Sessie sessie)
        {
            if (selectedTijdvakken == null)
            {
                sessie.SessieTijdvakken = new List<SessieTijdvak>();
                return;
            }

            var selectedTijdvakkenHS = new HashSet<string>(selectedTijdvakken);
            var sessieTijdvakken = new HashSet<int>(sessie.SessieTijdvakken.Select(c => c.Tijdvak.Id));
            foreach (var tijdvak in _context.Tijdvakken)
            {
                if (selectedTijdvakkenHS.Contains(tijdvak.Id.ToString()))
                {
                    if (!sessieTijdvakken.Contains(tijdvak.Id))
                    {
                        sessie.SessieTijdvakken.Add(new SessieTijdvak { SessieId = sessie.Id, TijdvakId = tijdvak.Id });
                    }
                }
                else
                {
                    if (sessieTijdvakken.Contains(tijdvak.Id))
                    {
                        SessieTijdvak verwijderSessieTijdvak = sessie.SessieTijdvakken.SingleOrDefault(i => i.TijdvakId == tijdvak.Id);
                        _context.Remove(verwijderSessieTijdvak);
                    }
                }
            }
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