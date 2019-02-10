namespace AuctionSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class Category
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.Category.NameMaxLength)]
        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
