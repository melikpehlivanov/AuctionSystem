namespace AuctionSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Microsoft.AspNetCore.Identity;

    public class AuctionUser : IdentityUser
    {
        [Required]
        [MaxLength(ModelConstants.User.FullNameMaxLength)]
        public string FullName { get; set; }

        public ICollection<Item> ItemsSold { get; set; }
        public ICollection<Bid> Bids { get; set; }
    }
}
