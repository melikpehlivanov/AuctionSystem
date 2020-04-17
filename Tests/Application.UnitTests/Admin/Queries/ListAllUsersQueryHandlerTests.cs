namespace Application.UnitTests.Admin.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using AutoMapper;
    using Xunit;
    using Application.Admin.Queries.List;
    using FluentAssertions;
    using Moq;
    using Setup;
    using Application.Common.Models;
    using Microsoft.EntityFrameworkCore;
    using AuctionSystem.Infrastructure;
    using AuctionSystem.Infrastructure.Identity;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;

    [Collection("QueryCollection")]
    public class ListAllUsersQueryHandlerTests
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly IUserManager userManagerService;
        private readonly Mock<UriService> uriService;
        private readonly Mock<UserManager<AuctionUser>> mockedUserManager;

        public ListAllUsersQueryHandlerTests(QueryTestFixture fixture)
        {
            this.context = fixture.Context;
            this.mapper = fixture.Mapper;
            this.uriService = new Mock<UriService>("https://localhost:40000/api/items?id=5");
            this.mockedUserManager = IdentityMocker.GetMockedUserManager();

            this.userManagerService = new UserManagerService(
                this.mockedUserManager.Object,
                IdentityMocker.GetMockedRoleManager().Object,
                this.context);
        }

        [Fact]
        public async Task GetUsers_Should_Return_Correct_Count()
        {
            this.mockedUserManager
                .Setup(x => x.GetUsersInRoleAsync(AppConstants.AdministratorRole))
                .ReturnsAsync(new List<AuctionUser> { new AuctionUser { Id = Guid.NewGuid().ToString() } });
            var handler = new ListAllUsersQueryHandler(this.context, this.mapper, this.uriService.Object, this.userManagerService);

            var result = await handler.Handle(new ListAllUsersQuery(), CancellationToken.None);

            result
                .Should()
                .BeOfType<PagedResponse<ListAllUsersResponseModel>>();

            result
                .Data
                .Should()
                .HaveCount(await this.context.Users.CountAsync());
        }
    }
}
