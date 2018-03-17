using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Models;
using RdwTechdayRegistration.Models.UserViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RdwTechdayRegistration.Utility;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RdwTechdayRegistration.Controllers

{
    [Authorize(Roles = "User")]
    public class UsersController : Controller
    {
        private readonly RdwTechdayRegistration.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UsersController(RdwTechdayRegistration.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Deelnemers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = await _context.ApplicationUsers
                .OrderBy(u => u.Name)
                .ToListAsync();
            // brute force, maybe refactor if too slow
            foreach (ApplicationUser user in users)
            {
                user.isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            }
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EnterBadges()
        {
            List<BadgeContentModel> badges = new List<BadgeContentModel>();
            for (int i = 0; i < 10; i++ )
            {
                badges.Add(new BadgeContentModel() { PersonType = BadgePersonType.speaker });
            }


            return View(badges);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EnterBadges(List<BadgeContentModel> badges)
        {
            return BadgesResult(badges);    
        }

        private BadgeGenerator GetBadgeGenerator(Stream stream)
        {
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/badge_banner.jpg");
            return new BadgeGenerator(stream, imagePath);
        }
        private FileStreamResult BadgesResult(List<ApplicationUser> users)
        {
            Stream stream = new MemoryStream();
            BadgeGenerator bg = GetBadgeGenerator(stream);
            bg.FillPages(users);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "badges.pdf");
        }

        private FileStreamResult BadgesResult(List<BadgeContentModel> badges)
        {
            Stream stream = new MemoryStream();
            BadgeGenerator bg = GetBadgeGenerator(stream);
            bg.FillPages(badges);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "badges.pdf");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Badges()
        {
            List<ApplicationUser> users = await _context.ApplicationUsers
                .OrderBy(u => u.Name)
                .Where(u => u.EmailConfirmed)
                .Include(i => i.ApplicationUserTijdvakken)
                    .ThenInclude(i => i.Sessie)
                        .ThenInclude(i => i.Track)
                .Include(i => i.ApplicationUserTijdvakken)
                    .ThenInclude(i => i.Tijdvak)
                .ToListAsync();
            // brute force, maybe refactor if too slow
            foreach (ApplicationUser user in users)
            {
                user.isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            }
            return BadgesResult(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Badge(string id)
        {
            var user = await _context.ApplicationUsers
                .Where(u => u.EmailConfirmed)
                .Include(i => i.ApplicationUserTijdvakken)
                    .ThenInclude(i => i.Sessie)
                        .ThenInclude(i => i.Track)
                .Include(i => i.ApplicationUserTijdvakken)
                    .ThenInclude(i => i.Tijdvak)
                .SingleOrDefaultAsync(m => m.Id == id);

            user.isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            return BadgesResult(new List<ApplicationUser> { user });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .SingleOrDefaultAsync(m => m.Id == id);

            List<ApplicationUserTijdvak> atv = await _context.ApplicationUserTijdvakken
                .Where(t => t.ApplicationUserId == user.Id)
                .Where(t => t.SessieId != null)
                .Include( t => t.Sessie)
                    .ThenInclude(t => t.Track)
                .Include(t => t.Tijdvak)
                .OrderBy(t => t.Tijdvak.Order)
                .ToListAsync();

            ViewBag.ApplicationUserTijdvakken = atv;

            if (user== null)
            {
                return NotFound();
            }

            return View(user);
        }


        // GET: Deelnemers/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var deelnemer = await _context.ApplicationUsers.SingleOrDefaultAsync(m => m.Id == id);
            await _userManager.DeleteAsync(deelnemer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdminRoleConfirmed(string id)
        {
            ApplicationUser deelnemer = await _context.ApplicationUsers.SingleOrDefaultAsync(m => m.Id == id);

            if (!(await _userManager.IsInRoleAsync(deelnemer, "Admin")))
            {
                await _userManager.AddToRoleAsync(deelnemer, "Admin");
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RevokeAdminRoleConfirmed(string id)
        {
            var deelnemer = await _context.ApplicationUsers.SingleOrDefaultAsync(m => m.Id == id);
            if (await _userManager.IsInRoleAsync(deelnemer, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(deelnemer, "Admin");
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SelectSessies()
        {
            string id = _userManager.GetUserId(User);
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _context.ApplicationUsers
                .Include(c => c.ApplicationUserTijdvakken)
                    .ThenInclude(atv => atv.Sessie)
                        .ThenInclude(t => t.Track)
                .Include(c => c.ApplicationUserTijdvakken)
                    .ThenInclude(atv => atv.Sessie)
                        .ThenInclude(t => t.Ruimte)
                .Include(c => c.ApplicationUserTijdvakken)
                    .ThenInclude(atv => atv.Sessie)
                        .ThenInclude(t => t.SessieTijdvakken)
                .Include(c => c.ApplicationUserTijdvakken)
                     .ThenInclude(t => t.Tijdvak)
                .SingleOrDefaultAsync(m => m.Id == id);

            SelectSessies model = new SelectSessies();
            model.ApplicationUserTijdvakken = user.ApplicationUserTijdvakken.OrderBy(atv => atv.Tijdvak.Order).ToList();

            if (user == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditSessionSelection(int? tijdvakid, int? sessieid)
        {
            string id = _userManager.GetUserId(User);
            if (id == null || tijdvakid == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _context.ApplicationUsers
                .Include(c => c.ApplicationUserTijdvakken)
                    .ThenInclude(atv => atv.Sessie)
                .Include(c => c.ApplicationUserTijdvakken)
                     .ThenInclude(t => t.Tijdvak)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // now find the sessies with this tijdvak attached
            List<SessieTijdvak> sessionTijdvakken = await _context.SessieTijdvakken
                .Where(t => t.Tijdvak.Id == tijdvakid)
                .Include(s => s.Sessie)
                    .ThenInclude(t => t.Track)
                .Include(s => s.Sessie)
                    .ThenInclude(t => t.Ruimte)
                .Include(s => s.Sessie)
                    .ThenInclude(t => t.SessieTijdvakken)
                .Include(t => t.Tijdvak)
                .OrderBy(t => t.Sessie.Track.Naam)
                .ToListAsync();

            // next we construct a list of sessies that can be selected bij the user
            // if all tijdvakken of the sessie are available in the user tijdvakken
            // then sessie van be offered to the user.

            List<Sessie> availableSessies = new List<Sessie>();
            List<Tijdvak> userTakenTijdvakken = new List<Tijdvak>();

            // construct a list of tijdvakken that have been taken by previous selections of this user
            foreach (var utv in user.ApplicationUserTijdvakken)
            {
                // ignore tijdvak without a sessie, the current tijdvak and the tijdvakken taken by the current session
                // as we ar replacing the session value with something new
                if (utv.Sessie != null && utv.TijdvakId != tijdvakid && utv.SessieId != sessieid)
                {
                    userTakenTijdvakken.Add(utv.Tijdvak);
                }
            }

            foreach (SessieTijdvak stv in sessionTijdvakken)
            {
                bool isFree = true;

                // check for the tijdvakken of the sessie if the space they need timewise is available in the tijdvakken of the user.
                foreach (SessieTijdvak neededTijdvak in stv.Sessie.SessieTijdvakken)
                {
                    if (userTakenTijdvakken.Where(t => t.Id == neededTijdvak.TijdvakId).Count() != 0)
                    {
                        isFree = false;
                    }
                }
                if (isFree)
                {
                    availableSessies.Add(stv.Sessie);
                }
            }

            EditSessionSelection model = new EditSessionSelection { Sessies = availableSessies, TijdvakId = tijdvakid, CurrentSessionId = (int)sessieid };
            model.UserCounts = await Sessie.GetUserCountsAsync(_context);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveSession(EditSessionSelection model)
        {
            string id = _userManager.GetUserId(User);
            if (model == null || id == null || model.TijdvakId == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _context.ApplicationUsers
                .Include(c => c.ApplicationUserTijdvakken)
                    .ThenInclude(atv => atv.Sessie)
                .Include(c => c.ApplicationUserTijdvakken)
                     .ThenInclude(t => t.Tijdvak)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            Sessie sessie = await _context.Sessies
                .Include(s => s.SessieTijdvakken)
                .SingleOrDefaultAsync(s => s.Id == model.SelectedSessieId);

            if (sessie == null)
            {
                return NotFound();
            }

            foreach (ApplicationUserTijdvak autv in user.ApplicationUserTijdvakken)
            {
                if (autv.SessieId == sessie.Id)
                {
                    autv.SessieId = null;
                }
            }
            TempData["StatusMessage"] = "U bent uitgeschreven voor de sessie...";

            var saveresult = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SelectSessies));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSessionSelection(EditSessionSelection model)
        {
            string id = _userManager.GetUserId(User);
            if (model == null || id == null || model.TijdvakId == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _context.ApplicationUsers
                .Include(c => c.ApplicationUserTijdvakken)
                    .ThenInclude(atv => atv.Sessie)
                .Include(c => c.ApplicationUserTijdvakken)
                     .ThenInclude(t => t.Tijdvak)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            Sessie sessie = await _context.Sessies
                .Include(s => s.SessieTijdvakken)
                .Include(s => s.Ruimte)
                .SingleOrDefaultAsync(s => s.Id == model.SelectedSessieId);

            if (sessie == null)
            {
                return NotFound();
            }

            int usercount = await sessie.GetUserCountAsync(_context);
            TempData["StatusMessage"] = "Fout: Registreren bij sessie niet gelukt, probeer het nog een keer...";

            if (usercount < sessie.Ruimte.Capacity)
            {
                // release tijdvakken for the current session (as we might be going from a 2 tijdvak session to a 1 tijdvak session, we need to make sure all tijdvakken are released)
                ApplicationUserTijdvak currentautv = await _context.ApplicationUserTijdvakken
                    .Where(t => t.ApplicationUserId == user.Id)
                    .SingleOrDefaultAsync(t => t.TijdvakId == model.TijdvakId);
                Sessie currentsessie = await _context.Sessies.SingleOrDefaultAsync(s => s.Id == currentautv.SessieId);
                if (currentsessie != null)
                {
                    foreach (ApplicationUserTijdvak autv in user.ApplicationUserTijdvakken)
                    {
                        if (autv.SessieId == currentsessie.Id)
                        {
                            autv.SessieId = null;
                        }
                    }
                }

                // reserve tijdvakken for the selected session
                foreach (SessieTijdvak stv in sessie.SessieTijdvakken)
                {
                    foreach (ApplicationUserTijdvak autv in user.ApplicationUserTijdvakken)
                    {
                        if (stv.TijdvakId == autv.TijdvakId)
                        {
                            autv.SessieId = stv.SessieId;
                        }
                    }
                }
                try
                {
                    sessie.ForceChangeCount++;
                    var saveresult = await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "U bent geregistreerd voor de sessie...";
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }
            return RedirectToAction(nameof(SelectSessies));
        }
    }
}