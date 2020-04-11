namespace Domain.Entities
{
    using System.Collections.Generic;

    public class SubCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }
        
        public ICollection<Item> Items { get; set; }
    }
}
