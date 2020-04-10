namespace AuctionSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Picture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string ItemId { get; set; }

        public Item Item { get; set; }
    }
}
