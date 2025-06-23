using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabInventory.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LabInventory.Models{
    public class Inventory{
        [Key]
        public Guid id { get; set;}  = Guid.NewGuid();
        public String? Name { get; set;}
        public String? Description { get; set;}
        public int Quantity { get; set;}
        public Decimal Weight{ get; set;}
        public DateTime CreateDate{ get; set;} = DateTime.Now;

        [ForeignKey("Type")]
        public Guid TypeId { get; set;}
        [ValidateNever]
        public Type Type{ get; set;}

    }
}