namespace AuctionSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class Bid
    {
        public string Id { get; set; }

        [Required]
        [Range(typeof(decimal), ModelConstants.Bid.MinAmount, ModelConstants.Bid.MaxAmount)]
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
