namespace Api.Models
{
    using Application.Admin.Queries.List;
    using global::Common.AutoMapping.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class UsersFilter : IMapWith<ListAllUsersQueryFilter>
    {
        [FromQuery(Name = "userId")]
        public string UserId { get; set; }
    }
}