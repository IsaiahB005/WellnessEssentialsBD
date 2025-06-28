using LabInventory.Models;
using LabInventory.ViewModels;
using LabInventory.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using log4net;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LabInventory.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}