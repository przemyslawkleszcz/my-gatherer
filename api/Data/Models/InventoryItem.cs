using Microsoft.EntityFrameworkCore;

namespace my_gatherer_api.Data.Models
{
    public class InventoryItem
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public int Quantity { get; set; }

        public virtual CardItem CardItem { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryItem>().HasKey(t => new { t.UserId, t.Id });
            modelBuilder.Entity<InventoryItem>().HasOne(x => x.CardItem).WithMany(x => x.InventoryItems).HasForeignKey(x=>x.Id);
        }
    }
}
