namespace AuctionSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Bid
    {
        public string Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string UserId { get; set; }

        public AuctionUser User { get; set; }

        [Required]
        public string ItemId { get; set; }

        public Item Item { get; set; }

        [Required]
        public DateTime MadeOn { get; set; }
    }
}
