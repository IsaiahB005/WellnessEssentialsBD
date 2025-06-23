using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabInventory.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LabInventory.Models{
    public class Order{
        public Guid id { get; set; } = Guid.NewGuid();
        public int OrderNumber { get; set; }
        public int Qty { get; set; }
        public DateTime CreateDate{ get; set; }= DateTime.Now;
        public Decimal PaymentReceived { get; set; }
        public Boolean IsDelivered { get; set; }

        [ForeignKey("Inventory")]
        [Required]
        public Guid InventoryId{ get; set; }

        [ForeignKey("Shipment")]
        public Guid? ShipmentId { get; set; }

        [ForeignKey("Customer")]
        [Required]
        public Guid CustomerId{ get; set; }

        [ValidateNever]
        public Shipment Shipment{ get; set; }
        [ValidateNever]
        public Inventory Inventory{ get; set; }
        [ValidateNever]
        public Customer Customer{ get; set; }
    }

}