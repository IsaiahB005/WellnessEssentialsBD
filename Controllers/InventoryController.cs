using LabInventory.Models;
using LabInventory.ViewModels;
using LabInventory.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using log4net;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LabInventory.Controllers{
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TypeController));

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {

            var inventoryVm = _context.Inventory
                .Include(i => i.Type)
                .Select(i => new InventoryViewModel
                {
                    Inventory = i,
                    ItemBuyPrice = _context.Prices
                    .Where(p => p.InventoryId == i.id && p.IsActive)
                    .OrderByDescending(p => p.id)
                    .Select(p => p.ItemBuyPrice)
                    .FirstOrDefault(),
                    
                    ItemSellPrice = _context.Prices
                    .Where(p => p.InventoryId == i.id && p.IsActive)
                    .OrderByDescending(p => p.id)
                    .Select(p => p.ItemSellPrice)
                    .FirstOrDefault()

                })
                .ToList();

            foreach (var item in inventoryVm)
            {

                item.TotalCost = item.Inventory.Quantity * item.ItemBuyPrice;
            }

            return View(inventoryVm);
        }

        // GET: Inventory/CreateInventory
        public ActionResult CreateInventory()
        {
            var types = _context.Types.ToList();

            var model = new InventoryViewModel
            {
                Inventory = new Inventory(),
                Types = types,
                TypeSelectList = types.Select(t => new SelectListItem
                {
                    Value = t.id.ToString(),
                    Text = t.Name
                })
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInventory(InventoryViewModel model)
        {

            if (!ModelState.IsValid)
            {
                // Debug line to confirm this path
                Console.WriteLine("Model is invalid");
            }

            if (ModelState.IsValid)
            {
                // Save Inventory
                var inventory = model.Inventory;
                inventory.CreateDate = DateTime.Now;
                _context.Inventory.Add(inventory);

                // Save related Price
                var price = new Price
                {
                    InventoryId = inventory.id,
                    IsActive = true,
                    ItemBuyPrice = model.ItemBuyPrice,
                    ItemSellPrice = model.ItemSellPrice
                };
                _context.Prices.Add(price);

                _context.SaveChanges();
                Console.WriteLine("Model is valid");
                return RedirectToAction("Index");
            }

            // Rebuild select list
            var types = _context.Types.ToList();
            model.Types = types;
            model.TypeSelectList = types.Select(t => new SelectListItem
            {
                Value = t.id.ToString(),
                Text = t.Name
            });
            return View(model);
        }
        
        // GET: Inventory/Edit/5
        public IActionResult Edit(Guid id)
        {
            var inventory = _context.Inventory.Find(id);
            if (inventory == null) return NotFound();

            var price = _context.Prices
                .Where(p => p.InventoryId == id && p.IsActive)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();

            var viewModel = new InventoryViewModel
            {
                Inventory = inventory,
                ItemBuyPrice = price?.ItemBuyPrice ?? 0,
                ItemSellPrice = price?.ItemSellPrice ?? 0,
                Types = _context.Types.ToList(),
                TypeSelectList = _context.Types.Select(t => new SelectListItem
                {
                    Value = t.id.ToString(),
                    Text = t.Name
                })
            };

            return View(viewModel);
        }


        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, InventoryViewModel model)
        {
            if (id != model.Inventory.id) return NotFound();

            if (!ModelState.IsValid)
            {
                model.Types = _context.Types.ToList();
                model.TypeSelectList = model.Types.Select(t => new SelectListItem
                {
                    Value = t.id.ToString(),
                    Text = t.Name
                });
                return View(model);
            }

            try
            {
                // Update inventory
                _context.Update(model.Inventory);

                // Deactivate previous prices
                var existingPrices = _context.Prices
                    .Where(p => p.InventoryId == model.Inventory.id && p.IsActive)
                    .ToList();

                foreach (var p in existingPrices)
                {
                    p.IsActive = false;
                    _context.Prices.Update(p);
                }

                // Add new price
                var newPrice = new Price
                {
                    InventoryId = model.Inventory.id,
                    IsActive = true,
                    ItemBuyPrice = model.ItemBuyPrice,
                    ItemSellPrice = model.ItemSellPrice
                };
                _context.Prices.Add(newPrice);

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Inventory.Any(e => e.id == id)) return NotFound();
                else throw;
            }
        }


        // GET: Inventory/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var inventory = _context.Inventory.Include(i => i.Type)
                .FirstOrDefault(m => m.id == id);
            if (inventory == null) return NotFound();

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var inventory = _context.Inventory.Find(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        
    }
}

