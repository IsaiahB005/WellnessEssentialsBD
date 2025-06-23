
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabInventory.Models{
    public class Type{
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        
    }
}