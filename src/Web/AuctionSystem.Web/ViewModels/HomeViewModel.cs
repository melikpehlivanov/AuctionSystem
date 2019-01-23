namespace AuctionSystem.Web.ViewModels
{
    using System.Collections.Generic;
    using Category;

    public class HomeViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}