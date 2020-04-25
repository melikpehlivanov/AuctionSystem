namespace Domain.Entities
{
    using System;
    using Common;

    public class Picture : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Url { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; }
    }
}
