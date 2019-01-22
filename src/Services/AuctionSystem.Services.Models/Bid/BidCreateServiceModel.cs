namespace AuctionSystem.Services.Models.Bid
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AuctionSystem.Models;

    public class BidCreateServiceModel : BaseBidServiceModel
    {
        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
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
