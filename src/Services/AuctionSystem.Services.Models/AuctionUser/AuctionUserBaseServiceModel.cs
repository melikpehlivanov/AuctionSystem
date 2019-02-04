namespace AuctionSystem.Services.Models.AuctionUser
{
    using AuctionSystem.Models;
    using Common.AutoMapping.Interfaces;
    using Microsoft.AspNetCore.Identity;

    public abstract class BaseAuctionUserServiceModel : IdentityUser, IMapWith<AuctionUser>
    {
        public string FullName { get; set; }
    }
}