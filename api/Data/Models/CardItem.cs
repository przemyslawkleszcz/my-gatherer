using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace my_gatherer_api.Data.Models
{
    public class CardItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public string SetName { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Card> Cards { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardItem>().HasKey(t => new { t.Id });
        }
    }
}
