namespace Application.Admin.Queries.List
{
    using System.Collections.Generic;
    using Domain.Entities;
    using global::Common.AutoMapping.Interfaces;

    public class ListAllUsersResponseModel : IMapWith<AuctionUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public ICollection<string> CurrentRoles { get; set; }

        public ICollection<string> NonCurrentRoles { get; set; }
    }
}
