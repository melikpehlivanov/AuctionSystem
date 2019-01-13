namespace AuctionSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Item
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal StartingPrice { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal MinIncrease { get; set; }

        [Required]
        public DateTime StartTime { get; set;}

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        public string UserId { get; set; }

        public AuctionUser User { get; set; }
    }
}
