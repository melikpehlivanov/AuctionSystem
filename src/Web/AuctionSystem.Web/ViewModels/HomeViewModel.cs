namespace AuctionSystem.Web.ViewModels
{
    using System.Collections.Generic;
    using Category;
    using Item;

    public class HomeViewModel
    {
        public IEnumerable<HottestItemViewModel> HottestItems { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}