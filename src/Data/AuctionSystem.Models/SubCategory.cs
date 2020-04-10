namespace AuctionSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;

    public class SubCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.SubCategory.NameMaxLength)]
        public string Name { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; }
        
        public ICollection<Item> Items { get; set; }
    }
}
