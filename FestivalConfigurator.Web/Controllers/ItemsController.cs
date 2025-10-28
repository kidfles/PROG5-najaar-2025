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
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(ItemType? type, string? search, string? sort)
        {
            ViewData["CurrentType"] = type;
            ViewData["CurrentSearch"] = search;
            ViewData["NameSort"] = sort == "name_desc" ? "name_asc" : "name_desc";
            ViewData["TypeSort"] = sort == "type_desc" ? "type_asc" : "type_desc";
            ViewData["PriceSort"] = sort == "price_desc" ? "price_asc" : "price_desc";

            IQueryable<Item> query = _context.Items.AsNoTracking();

            if (type.HasValue)
            {
                query = query.Where(i => i.ItemType == type.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i => i.Name.Contains(search));
            }

            query = sort switch
            {
                "name_asc" => query.OrderBy(i => i.Name),
                "name_desc" => query.OrderByDescending(i => i.Name),
                "type_asc" => query.OrderBy(i => i.ItemType).ThenBy(i => i.Name),
                "type_desc" => query.OrderByDescending(i => i.ItemType).ThenBy(i => i.Name),
                "price_asc" => query.OrderBy(i => i.Price).ThenBy(i => i.Name),
                "price_desc" => query.OrderByDescending(i => i.Price).ThenBy(i => i.Name),
                _ => query.OrderBy(i => i.ItemType).ThenBy(i => i.Name)
            };

            var items = await query.ToListAsync();
            return View(items);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ItemType,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ItemType,Price")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Kan item niet verwijderen: het item is gebruikt in pakketten.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
