namespace AuctionSystem.Web.Areas.Admin.Models.Users
{
    using System.Collections.Generic;

    public class UserListingViewModel //: IMapWith<UserListingServiceModel>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public ICollection<string> CurrentRoles { get; set; }

        public ICollection<string> NonCurrentRoles { get; set; }
    }
}
