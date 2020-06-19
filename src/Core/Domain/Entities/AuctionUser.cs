namespace Domain.Entities
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class AuctionUser : IdentityUser
    {
        public string FullName { get; set; }

        public ICollection<Item> ItemsSold { get; set; } = new HashSet<Item>();

        public ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();
    }
}