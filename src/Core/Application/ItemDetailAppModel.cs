namespace Application
{
    using System;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;

    public class ItemDetailAppModel : IMapWith<Item>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal MinIncrease { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan RemainingTime => this.EndTime - DateTime.UtcNow;

        public string UserUserName { get; set; }
    }
}
