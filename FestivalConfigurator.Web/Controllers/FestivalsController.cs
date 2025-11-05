using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FestivalConfigurator.Domain;
using FestivalConfigurator.Infrastructure;
using FestivalConfigurator.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FestivalConfigurator.Web.Controllers
{
    public class FestivalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FestivalsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Festivals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Festivals.ToListAsync());
        }

        // GET: Festivals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var festival = await _context.Festivals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (festival == null)
            {
                return NotFound();
            }

            return View(festival);
        }

        // GET: Festivals/Create
        public IActionResult Create()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var vm = new FestivalFormViewModel
            {
                StartDate = today,
                EndDate = today.AddDays(1)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FestivalFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (logoSaved, logoPath) = await TrySaveLogoAsync(model.LogoFile);
            if (!logoSaved)
            {
                model.ExistingLogoPath = null;
                return View(model);
            }

            var festival = new Festival
            {
                Name = model.Name,
                Place = model.Place,
                Description = model.Description,
                BasicPrice = model.BasicPrice,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Logo = logoPath
            };

            _context.Add(festival);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Festivals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var festival = await _context.Festivals.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            if (festival == null)
            {
                return NotFound();
            }

            var vm = new FestivalFormViewModel
            {
                Id = festival.Id,
                Name = festival.Name,
                Place = festival.Place,
                Description = festival.Description,
                BasicPrice = festival.BasicPrice,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                ExistingLogoPath = festival.Logo
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FestivalFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var festival = await _context.Festivals.FindAsync(id);
            if (festival == null)
            {
                return NotFound();
            }

            var (logoSaved, logoPath) = await TrySaveLogoAsync(model.LogoFile);
            if (!logoSaved)
            {
                model.ExistingLogoPath = festival.Logo;
                return View(model);
            }

            festival.Name = model.Name;
            festival.Place = model.Place;
            festival.Description = model.Description;
            festival.BasicPrice = model.BasicPrice;
            festival.StartDate = model.StartDate;
            festival.EndDate = model.EndDate;
            festival.Logo = logoPath ?? festival.Logo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FestivalExists(festival.Id))
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

        // GET: Festivals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var festival = await _context.Festivals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (festival == null)
            {
                return NotFound();
            }

            return View(festival);
        }

        // POST: Festivals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var festival = await _context.Festivals.FindAsync(id);
            if (festival == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Festivals.Remove(festival);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Kan festival niet verwijderen: er zijn nog gekoppelde pakketten of items.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<(bool Saved, string? Path)> TrySaveLogoAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return (true, null);
            }

            var isPng = string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(Path.GetExtension(file.FileName), ".png", StringComparison.OrdinalIgnoreCase);

            if (!isPng)
            {
                ModelState.AddModelError(nameof(FestivalFormViewModel.LogoFile), "Upload een PNG-bestand (.png).");
                return (false, null);
            }

            var logosFolder = Path.Combine(_environment.WebRootPath, "img", "logos");
            Directory.CreateDirectory(logosFolder);

            var fileName = $"{Guid.NewGuid():N}.png";
            var fullPath = Path.Combine(logosFolder, fileName);

            await using var stream = System.IO.File.Create(fullPath);
            await file.CopyToAsync(stream);

            return (true, $"/img/logos/{fileName}");
        }

        private bool FestivalExists(int id)
        {
            return _context.Festivals.Any(e => e.Id == id);
        }
    }
}
