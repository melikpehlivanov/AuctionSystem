namespace MvcWeb.Areas.Admin.Models.Users
{
    using System.Collections.Generic;
    using Application.Admin.Queries.List;
    using Common.AutoMapping.Interfaces;

    public class UserListingViewModel : IMapWith<ListAllUsersResponseModel>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public ICollection<string> CurrentRoles { get; set; }

        public ICollection<string> NonCurrentRoles { get; set; }
    }
}
