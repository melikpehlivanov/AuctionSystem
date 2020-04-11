namespace Domain.Entities
{
    using System.Collections.Generic;
    using Common;

    public class SubCategory : AuditableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Item> Items { get; set; } = new HashSet<Item>();
    }
}
