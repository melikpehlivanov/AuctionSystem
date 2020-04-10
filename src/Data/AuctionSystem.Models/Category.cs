namespace AuctionSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;

    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [MaxLength(ModelConstants.Category.NameMaxLength)]
        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
