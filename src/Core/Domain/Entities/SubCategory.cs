namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using Common;

    public class SubCategory : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Item> Items { get; set; } = new HashSet<Item>();
    }
}