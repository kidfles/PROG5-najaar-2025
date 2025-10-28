using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FestivalConfigurator.Domain;
using FestivalConfigurator.Infrastructure;

namespace FestivalConfigurator.Web.Controllers
{
    public class FestivalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FestivalsController(ApplicationDbContext context)
        {
            _context = context;
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
            return View();
        }

        // POST: Festivals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Place,Logo,Description,BasicPrice,StartDate,EndDate")] Festival festival)
        {
            if (ModelState.IsValid)
            {
                _context.Add(festival);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(festival);
        }

        // GET: Festivals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var festival = await _context.Festivals.FindAsync(id);
            if (festival == null)
            {
                return NotFound();
            }
            return View(festival);
        }

        // POST: Festivals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Place,Logo,Description,BasicPrice,StartDate,EndDate")] Festival festival)
        {
            if (id != festival.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(festival);
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
            return View(festival);
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

        private bool FestivalExists(int id)
        {
            return _context.Festivals.Any(e => e.Id == id);
        }
    }
}
