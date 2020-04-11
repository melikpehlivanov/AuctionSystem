namespace Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;
    using global::Common;

    public class SubCategory : AuditableEntity
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
