namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Item
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal MinIncrease { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsEmailSent { get; set; } = false;
        public string UserId { get; set; }

        public string SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public ICollection<Bid> Bids { get; set; }
        public ICollection<Picture> Pictures { get; set; }
    }
}
