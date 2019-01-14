namespace AuctionSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SubCategory
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; }
        
        public ICollection<Item> Items { get; set; }
    }
}
