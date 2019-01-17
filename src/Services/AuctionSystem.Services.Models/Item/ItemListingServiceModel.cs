namespace AuctionSystem.Services.Models.Item
{
    public class ItemListingServiceModel : BaseItemServiceModel
    {
        //TODO: Add picture when we implement id
        public string Title { get; set; }

        public decimal StartingPrice { get; set; }
    }
}
