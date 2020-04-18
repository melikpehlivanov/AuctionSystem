namespace Application.Admin.Queries.List
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Helpers;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class ListAllUsersQueryHandler : IRequestHandler<ListAllUsersQuery, PagedResponse<ListAllUsersResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly IUserManager userManager;

        public ListAllUsersQueryHandler(
            IAuctionSystemDbContext context,
            IMapper mapper,
            IUserManager userManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<PagedResponse<ListAllUsersResponseModel>> Handle(ListAllUsersQuery request, CancellationToken cancellationToken)
        {
            var skipCount = (request.PageNumber - 1) * request.PageSize;
            var queryable = this.context.Users.AsQueryable();

            var adminIds = await this.userManager.GetUsersInRoleAsync(AppConstants.AdministratorRole);

            if (request?.Filters == null)
            {
                var pagedUsers = PaginationHelpers.CreatePaginatedResponse(request, await queryable
                    .Skip(skipCount)
                    .Take(request.PageSize)
                    .ProjectTo<ListAllUsersResponseModel>(this.mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken), await this.context.Users.CountAsync(cancellationToken));

                AddUserRoles(pagedUsers.Data, adminIds);
                return pagedUsers;
            }

            queryable = AddFiltersOnQuery(request.Filters, queryable);
            var users = await queryable
                .Skip(skipCount)
                .Take(request.PageSize)
                .ProjectTo<ListAllUsersResponseModel>(this.mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            var result = PaginationHelpers.CreatePaginatedResponse(request, users, await this.context.Users.CountAsync(cancellationToken));
            AddUserRoles(users, adminIds);

            return result;
        }

        private static void AddUserRoles(IEnumerable<ListAllUsersResponseModel> users, IEnumerable<string> adminIds)
        {
            foreach (var user in users)
            {
                var currentUserRoles = new List<string>();
                var nonCurrentRoles = new List<string>();

                if (adminIds.Contains(user.Id))
                {
                    currentUserRoles.Add(AppConstants.AdministratorRole);
                }
                else
                {
                    nonCurrentRoles.Add(AppConstants.AdministratorRole);
                }

                user.CurrentRoles = currentUserRoles;
                user.NonCurrentRoles = nonCurrentRoles;
            }
        }

        private static IQueryable<AuctionUser> AddFiltersOnQuery(ListAllUsersQueryFilter filters, IQueryable<AuctionUser> queryable)
        {
            if (!string.IsNullOrEmpty(filters?.UserId))
            {
                queryable = queryable.Where(i => i.Id == filters.UserId);
            }

            return queryable;
        }
    }
}