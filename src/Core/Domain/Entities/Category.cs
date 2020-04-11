namespace Domain.Entities
{
    using System.Collections.Generic;
    using Common;

    public class Category : AuditableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
    }
}
