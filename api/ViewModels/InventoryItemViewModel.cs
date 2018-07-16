using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_gatherer_api.ViewModels
{
    public class InventoryItemViewModel
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public string SetName { get; set; }
        public string ImageUrl { get; set; }
    }
}
