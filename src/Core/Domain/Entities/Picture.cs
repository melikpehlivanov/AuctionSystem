namespace Domain.Entities
{
    using Common;

    public class Picture : AuditableEntity
    {
        public string Id { get; set; }
        public string Url { get; set; }

        public string ItemId { get; set; }
        public Item Item { get; set; }
    }
}
