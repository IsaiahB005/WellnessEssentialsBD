using LabInventory.Models;
using LabInventory.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using log4net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LabInventory.Controllers{
    public class TypeController : Controller
    {
        private readonly AppDbContext _context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TypeController));

        public TypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Type
        public IActionResult Index()
        {
            var types = _context.Types.ToList();
            return View(types);
        }

        // GET: /Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Type/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Models.Type type)
        {
            if (ModelState.IsValid)
            {
                _context.Types.Add(type);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }
        
        // GET: Type/Edit/5
        public IActionResult Edit(Guid id)
        {
            var type = _context.Types.Find(id);
            if (type == null)
            {
                return NotFound();
            }
            return View(type);
        }

        // POST: Type/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Models.Type type)
        {
            if (id != type.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(type);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        // GET: Type/Delete/guid
        public IActionResult Delete(Guid id)
        {
            var type = _context.Types.Find(id);
            if (type == null)
            {
                return NotFound();
            }
            return View(type);
        }

        // POST: Type/Delete/guid
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var type = _context.Types.Find(id);
            if (type != null)
            {
                _context.Types.Remove(type);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}