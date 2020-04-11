namespace Domain.Entities
{
    using System;

    public class Bid
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public DateTime MadeOn { get; set; }

        public string ItemId { get; set; }
        public Item Item { get; set; }
    }
}
