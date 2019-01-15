namespace AuctionSystem.Services.Models.Item
{
    using AuctionSystem.Models;
    using Common.AutoMapping.Interfaces;

    public abstract class BaseItemServiceModel : IMapWith<Item>
    {
        public string Id { get; set; }
    }
}