using LabInventory.Data;
using LabInventory.Models;
using LabInventory.ViewModels;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace LabInventory.Controllers{
    public class OrderController : Controller{
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context){
            _context = context;
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            var inventoryList = _context.Inventory.ToList();
            var customerList = _context.Customers.ToList();

            var viewModel = new OrderViewModel
            {
                InventorySelections = inventoryList.Select(i => new InventorySelection
                {
                    InventoryId = i.id,
                    InventoryName = i.Name,
                    Qty = 0
                }).ToList(),
                CustomerList = customerList.Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.CustomerName
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.InventoryList = _context.Inventory.Select(i => new SelectListItem
                {
                    Value = i.id.ToString(),
                    Text = i.Name
                }).ToList();
                
                model.CustomerList = _context.Customers.Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.CustomerName
                }).ToList();

                return View(model);
            }

            // Save Customer
            Customer customer;
            customer = _context.Customers.Find(model.SelectedCustomerId.Value);

            int newOrderNumber = GetNextOrderNumber();

            // Save Orders
            foreach (var selection in model.InventorySelections)
            {
                if (selection.Qty > 0) // only save items with quantity > 0
                {
                    var order = new Order
                    {
                        CustomerId = customer.id,
                        OrderNumber = newOrderNumber,
                        InventoryId = selection.InventoryId,
                        Qty = selection.Qty,
                        CreateDate = DateTime.Now,
                        PaymentReceived = model.PaymentReceived,
                        IsDelivered = model.IsDelivered,
                        ShipmentId = null
                    };

                    _context.Orders.Add(order);
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Inventory)
                .OrderBy(o => o.OrderNumber)
                .ToList();

            return View(orders);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var order = _context.Orders
                .Include(o => o.Inventory)
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.id == id);

            if (order == null)
            {
                return NotFound();
            }

            // If you want to allow changing Inventory, prepare dropdown
            ViewBag.InventoryList = _context.Inventory.Select(i => new SelectListItem
            {
                Value = i.id.ToString(),
                Text = i.Name
            }).ToList();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Order updatedOrder)
        {
            if (id != updatedOrder.id)
            {
                return NotFound();
            }

            var existingOrder = _context.Orders.FirstOrDefault(o => o.id == id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            // Only update editable fields; do NOT update CustomerId
            existingOrder.Qty = updatedOrder.Qty;
            existingOrder.PaymentReceived = updatedOrder.PaymentReceived;
            existingOrder.IsDelivered = updatedOrder.IsDelivered;
            existingOrder.InventoryId = updatedOrder.InventoryId;
            existingOrder.ShipmentId = updatedOrder.ShipmentId;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: Order/Delete/{id}
        public IActionResult Delete(Guid id)
        {
            var order = _context.Orders.Include(o => o.Customer).Include(o => o.Inventory)
                                    .FirstOrDefault(o => o.id == id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }


        private int GetNextOrderNumber()
        {
            var lastOrder = _context.Orders.OrderByDescending(o => o.OrderNumber).FirstOrDefault();
            return (lastOrder?.OrderNumber ?? 0) + 1;
        }
    }
}