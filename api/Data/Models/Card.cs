using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace my_gatherer_api.Data.Models
{
    public class Card
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public int Quantity { get; set; }

        public virtual CardItem CardItem { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().HasKey(t => new { t.UserId, t.Id });
            modelBuilder.Entity<Card>().HasOne(x => x.CardItem).WithMany(x => x.Cards).HasForeignKey(x=>x.Id);
        }
    }
}
