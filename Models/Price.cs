using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabInventory.Models;

namespace LabInventory.Models{
    public class Price{
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();
        public Boolean IsActive { get; set; }
        public Decimal ItemBuyPrice { get; set; }
        public Decimal ItemSellPrice { get; set; }
        [Required]
        [ForeignKey("Inventory")]
        public Guid InventoryId{ get; set; }

        public Inventory Inventory{ get; set; }
    }
}