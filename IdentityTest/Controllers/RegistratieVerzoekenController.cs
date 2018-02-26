using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;
using RdwTechdayRegistration.ValidationHelpers;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using IdentityTest.Data;
using Microsoft.AspNetCore.Authorization;

namespace RdwTechdayRegistration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegistratieVerzoekenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;


        public RegistratieVerzoekenController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet,HttpPost]
        public IActionResult VerifyEmail(string email)
        {
            if ( !EmailValidator.IsValid(email) )
            {
                return Json($"Vul een geldig e-mail adres in");
            }
            var registratieVerzoekCheck = _context.RegistratieVerzoeken.SingleOrDefault(m => m.Email == email);
            if (registratieVerzoekCheck != null)
            {
                return Json($"Dit e-mail adres is al geregistreerd");
            }

            return Json(true);
        }

        // GET: RegistratieVerzoeken
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegistratieVerzoeken.ToListAsync());
        }

        // GET: RegistratieVerzoeken/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registratieVerzoek = await _context.RegistratieVerzoeken
                .SingleOrDefaultAsync(m => m.Id == id);
            if (registratieVerzoek == null)
            {
                return NotFound();
            }

            return View(registratieVerzoek);
        }

        // GET: RegistratieVerzoeken/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistratieVerzoeken/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,Organisatie,Email,Telefoon,State")] RegistratieVerzoek registratieVerzoek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registratieVerzoek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registratieVerzoek);
        }

        public IActionResult RegistreerRDW()
        {
            return View();
        }

        // POST: RegistratieVerzoeken/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistreerRDW([Bind("Id,Naam,Email")] RegistratieVerzoek registratieVerzoek)
        {
            // Generieke checks op geldig email adres doen we client side
            //  check op geldig rdw adres kan alleen plaatsvinden in deze controller method omdat we gebruik maken van
            // de registrateverzoek class die ook voor niet RDW-ers gebruikt wordt
            if (registratieVerzoek.Email.Length < 7 || registratieVerzoek.Email.ToUpper().Substring(registratieVerzoek.Email.Length-7) != "@RDW.NL")
            {
                ModelState.AddModelError("Email", "Vul een RDW e-mail adres in.");
            }
            // check of email adres al gebruikt is
            var registratieVerzoekCheck = await _context.RegistratieVerzoeken.SingleOrDefaultAsync(m => m.Email == registratieVerzoek.Email);
            if ( registratieVerzoekCheck != null )
            {
                ModelState.AddModelError("Email", "Dit e-mail adres is al geregistreerd");
            }

            if (ModelState.IsValid)
            {
                registratieVerzoek.Organisatie = "RDW";
                _context.Add(registratieVerzoek);
                await _context.SaveChangesAsync();

                var apiKey = _configuration["sendgridkey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("rdwtechday@rdw.nl", "RDW Techday");
                var subject = " Bestig uw RDW Techday registratie";
                var to = new EmailAddress(registratieVerzoek.Email, registratieVerzoek.Naam);
                var plainTextContent = "Bevestig uw registratie met deze link: http://rdwtechdayregistration.azurewebsites.net/confirmregistration" + registratieVerzoek.Id + "/";
                var htmlContent = plainTextContent;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                return RedirectToAction(nameof(Index));
            }
            return View("RegistreerRDW",registratieVerzoek);
        }

        // GET: RegistratieVerzoeken/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registratieVerzoek = await _context.RegistratieVerzoeken.SingleOrDefaultAsync(m => m.Id == id);
            if (registratieVerzoek == null)
            {
                return NotFound();
            }
            return View(registratieVerzoek);
        }

        // POST: RegistratieVerzoeken/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,Organisatie,Email,Telefoon,State")] RegistratieVerzoek registratieVerzoek)
        {
            if (id != registratieVerzoek.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registratieVerzoek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistratieVerzoekExists(registratieVerzoek.Id))
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
            return View(registratieVerzoek);
        }

        // GET: RegistratieVerzoeken/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registratieVerzoek = await _context.RegistratieVerzoeken
                .SingleOrDefaultAsync(m => m.Id == id);
            if (registratieVerzoek == null)
            {
                return NotFound();
            }

            return View(registratieVerzoek);
        }

        // POST: RegistratieVerzoeken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registratieVerzoek = await _context.RegistratieVerzoeken.SingleOrDefaultAsync(m => m.Id == id);
            _context.RegistratieVerzoeken.Remove(registratieVerzoek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistratieVerzoekExists(int id)
        {
            return _context.RegistratieVerzoeken.Any(e => e.Id == id);
        }
    }
}
