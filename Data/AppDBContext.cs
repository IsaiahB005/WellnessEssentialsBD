using Microsoft.EntityFrameworkCore;
using LabInventory.Models;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LabInventory.Data{
    public class AppDbContext:DbContext{
        public AppDbContext(DbContextOptions<AppDbContext>options)  : base(options){

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Models.Type> Types { get; set; }

    }
}