namespace AuctionSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class SubCategory
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.SubCategory.NameMaxLength)]
        public string Name { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; }
        
        public ICollection<Item> Items { get; set; }
    }
}
