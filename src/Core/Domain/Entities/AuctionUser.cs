namespace Domain.Entities
{
    using System.Collections.Generic;

    public class AuctionUser
    {
        public string Id { get; set; }

        public string UserId { get; set; }


        public ICollection<Item> ItemsSold { get; set; } = new HashSet<Item>();
        public ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();
    }
}
