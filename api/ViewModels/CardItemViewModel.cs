using System.Linq;

namespace my_gatherer_api.ViewModels
{
    public class CardItemViewModel
    {
        public int Count => Items.Count();
        public int InInventory => Items.Count(x => x.InInventory);
        public IQueryable<CardItemViewData> Items { get; set; }
    }

    public class CardItemViewData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public bool InInventory { get; set; }
        public string ImageUrl { get; set; }
    }
}
