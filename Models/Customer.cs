using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabInventory.Models;

namespace LabInventory.Models{
    public class Customer{
        [Key]
        public Guid id{ get; set; } = Guid.NewGuid();
        [Required]
        public required String CustomerName { get; set; }
        [Required]
        public required String CustomerAddress{ get; set; }
        public String? PhoneNumber{ get; set; }
    }
}