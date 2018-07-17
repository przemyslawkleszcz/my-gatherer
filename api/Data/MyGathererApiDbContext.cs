using my_gatherer_api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace my_gatherer_api.Data
{
    public class MyGathererApiDbContext : DbContext
    {
        public MyGathererApiDbContext(DbContextOptions options)
        : base(options)
        {
            
        }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<CardItem> CardItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);
            InventoryItem.OnModelCreating(modelBuilder);
            CardItem.OnModelCreating(modelBuilder);
        }
    }
}
