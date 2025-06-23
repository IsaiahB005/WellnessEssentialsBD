using LabInventory.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LabInventory.ViewModels{
    public class InventoryViewModel
    {
        public Inventory Inventory { get; set; }
        [ValidateNever]
        public List<Models.Type> Types { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TypeSelectList { get; set; }

         // Pricing info
        public decimal ItemBuyPrice { get; set; }
        public decimal ItemSellPrice { get; set; }
        public decimal TotalCost { get; set; }
    }

}