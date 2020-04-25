namespace Domain.Entities
{
    using System;
    using Common;

    public class Bid : AuditableEntity
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime MadeOn { get; set; }

        public string UserId { get; set; }
        public AuctionUser User { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; }
    }
}
