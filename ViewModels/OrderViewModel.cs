using LabInventory.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LabInventory.ViewModels{
    public class InventorySelection
    {
        public Guid InventoryId { get; set; }
        public string InventoryName { get; set; } = string.Empty;
        public int Qty { get; set; }
    }

    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        // Customer details
        public string? CustomerName { get; set; }

        public string? CustomerAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public Decimal PaymentReceived { get; set; }
        public Boolean IsDelivered { get; set; }

        // Inventory selection
        [Required]
        public List<InventorySelection> InventorySelections { get; set; } = new();
        [ValidateNever]
        public List<SelectListItem> InventoryList { get; set; }
        
        // Customer fields
        public Guid? SelectedCustomerId { get; set; }
        public List<SelectListItem> CustomerList { get; set; } = new();
    }
}