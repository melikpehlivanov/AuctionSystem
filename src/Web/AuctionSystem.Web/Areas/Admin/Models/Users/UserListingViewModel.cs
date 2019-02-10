namespace AuctionSystem.Web.Areas.Admin.Models.User
{
    using System.Collections.Generic;
    using Common.AutoMapping.Interfaces;
    using Services.Models.AuctionUser;

    public class UserListingViewModel : IMapWith<UserListingServiceModel>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public ICollection<string> CurrentRoles { get; set; }

        public ICollection<string> NonCurrentRoles { get; set; }
    }
}
