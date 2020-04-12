namespace AuctionSystem.Infrastructure.Identity
{
    using Application.Common.Models;
    using Common.AutoMapping.Interfaces;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IMapWith<User>
    {
        public string FullName { get; set; }
    }
}
