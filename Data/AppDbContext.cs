using Microsoft.EntityFrameworkCore;
using InventorySystem.Models;

namespace InventorySystem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(
                "Host=localhost;Port=5432;Database=inventorydb;Username=postgres;Password=bh"
            );
        }
    }
}