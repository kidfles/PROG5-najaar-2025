using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FestivalConfigurator.Domain;
using FestivalConfigurator.Infrastructure;
using FestivalConfigurator.Web.Models;

namespace FestivalConfigurator.Web.Controllers
{
    public class PackagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PackagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Packages
        public async Task<IActionResult> Index(int? festivalId)
        {
            ViewData["FestivalId"] = festivalId;
            IQueryable<Package> query = _context.Packages.Include(p => p.Festival);
            if (festivalId.HasValue)
            {
                query = query.Where(p => p.FestivalId == festivalId.Value);
            }
            return View(await query.AsNoTracking().ToListAsync());
        }

        // GET: Packages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Packages
                .Include(p => p.Festival)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // GET: Packages/Create
        public IActionResult Create(int? festivalId)
        {
            ViewData["FestivalId"] = new SelectList(_context.Festivals, "Id", "Name", festivalId);
            return View(new Package { FestivalId = festivalId ?? 0 });
        }

        // POST: Packages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FestivalId,Name")] Package package)
        {
            if (ModelState.IsValid)
            {
                _context.Add(package);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { festivalId = package.FestivalId });
            }
            ViewData["FestivalId"] = new SelectList(_context.Festivals, "Id", "Name", package.FestivalId);
            return View(package);
        }

        // GET: Packages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            ViewData["FestivalId"] = new SelectList(_context.Festivals, "Id", "Name", package.FestivalId);
            return View(package);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FestivalId,Name")] Package package)
        {
            if (id != package.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(package);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageExists(package.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { festivalId = package.FestivalId });
            }
            ViewData["FestivalId"] = new SelectList(_context.Festivals, "Id", "Name", package.FestivalId);
            return View(package);
        }

        // GET: Packages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Packages
                .Include(p => p.Festival)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package != null)
            {
                _context.Packages.Remove(package);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { festivalId = package.FestivalId });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PackageExists(int id)
        {
            return _context.Packages.Any(e => e.Id == id);
        }

    // GET: /Packages/Ticket/5
    [HttpGet]
    public async Task<IActionResult> Ticket(int id)
    {
        var pkg = await _context.Packages
            .Include(p => p.Festival)
            .Include(p => p.PackageItems)
                .ThenInclude(pi => pi.Item)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (pkg == null) return NotFound("Pakket niet gevonden.");

        var catalogs = await _context.Items
            .AsNoTracking()
            .GroupBy(i => i.ItemType)
            .ToDictionaryAsync(g => g.Key, g => g.OrderBy(i => i.Name).ToList());

        var vm = new TicketViewModel
        {
            PackageId = pkg.Id,
            PackageName = pkg.Name,
            FestivalId = pkg.FestivalId,
            FestivalName = pkg.Festival.Name,
            FestivalPlace = pkg.Festival.Place,
            FestivalLogo = pkg.Festival.Logo,
            FestivalDescription = pkg.Festival.Description,
            StartDate = pkg.Festival.StartDate,
            EndDate = pkg.Festival.EndDate,
            BasicPrice = pkg.Festival.BasicPrice,
        };

        foreach (ItemType t in Enum.GetValues<ItemType>())
        {
            var existing = pkg.PackageItems.FirstOrDefault(pi => pi.Item.ItemType == t);

            var options = catalogs.TryGetValue(t, out var list)
                ? list.Select(i => new SelectListItem
                {
                    Text = $"{i.Name} ({i.Price:C})",
                    Value = i.Id.ToString(),
                    Selected = existing?.ItemId == i.Id
                }).ToList()
                : new List<SelectListItem>();

            vm.Panels.Add(new TicketPanelViewModel
            {
                Type = t,
                SelectedItemId = existing?.ItemId,
                SelectedItemName = existing?.Item.Name,
                UnitPrice = existing?.Item.Price,
                Quantity = existing?.Quantity ?? 0,
                Options = options,
                IconPath = $"/img/icons/{t}.svg"
            });
        }

        vm.PackageTotal = vm.BasicPrice + pkg.PackageItems.Sum(pi => pi.Item.Price * pi.Quantity);

        return View(vm);
    }

    // POST: /Packages/Ticket
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ticket(TicketPostModel form)
    {
        var pkg = await _context.Packages
            .Include(p => p.PackageItems)
                .ThenInclude(pi => pi.Item)
            .Include(p => p.Festival)
            .FirstOrDefaultAsync(p => p.Id == form.PackageId);
        if (pkg == null) return NotFound("Pakket niet gevonden.");

        var existing = pkg.PackageItems.FirstOrDefault(pi => pi.Item.ItemType == form.Type);

        if (string.Equals(form.Command, "deselect", StringComparison.OrdinalIgnoreCase) || form.ItemId is null || form.Quantity <= 0)
        {
            if (existing != null)
                _context.PackageItems.Remove(existing);
        }
        else
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == form.ItemId);
            if (item == null) return BadRequest("Dit item bestaat niet meer; je selectie is gewist.");
            if (item.ItemType != form.Type) return BadRequest("Ongeldige selectie voor dit type.");

            if (existing != null)
                _context.PackageItems.Remove(existing);

            _context.PackageItems.Add(new PackageItem
            {
                PackageId = pkg.Id,
                ItemId = item.Id,
                Quantity = form.Quantity
            });
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Ticket), new { id = pkg.Id });
    }
}
}
