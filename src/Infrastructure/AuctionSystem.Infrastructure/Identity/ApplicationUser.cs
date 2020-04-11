namespace AuctionSystem.Infrastructure.Identity
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(ModelConstants.User.FullNameMaxLength)]
        public string FullName { get; set; }

        public ICollection<Item> ItemsSold { get; set; }
        public ICollection<Bid> Bids { get; set; }
    }
}
