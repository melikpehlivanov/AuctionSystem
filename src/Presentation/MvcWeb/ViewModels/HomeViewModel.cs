namespace MvcWeb.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Category;
    using Item;

    public class HomeViewModel
    {
        public IEnumerable<LiveItemViewModel> LiveItems { get; set; }
        public IEnumerable<HottestItemViewModel> HottestItems { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public int CountOfLiveItems => this.LiveItems.Count();
    }
}