using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabInventory.Models;

namespace LabInventory.Models{
    public class Shipment{
        [Key]
        public Guid id{ get; set; } = Guid.NewGuid();
        public DateTime ShipmentDate{ get; set; }
        public Decimal TotalCost{ get; set; }
        public Boolean IsDone{ get; set; }
    }
}