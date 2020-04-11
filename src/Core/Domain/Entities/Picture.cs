namespace Domain.Entities
{
    public class Picture
    {
        public string Id { get; set; }
        public string Url { get; set; }

        public string ItemId { get; set; }
        public Item Item { get; set; }
    }
}
