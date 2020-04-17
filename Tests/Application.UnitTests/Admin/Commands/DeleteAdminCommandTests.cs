namespace Application.UnitTests.Admin.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Admin.Commands.DeleteAdmin;
    using AuctionSystem.Infrastructure.Identity;
    using Common.Exceptions;
    using Common.Interfaces;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using Setup;
    using Xunit;

    public class DeleteAdminCommandTests : CommandTestBase
    {
        private readonly IUserManager userManagerService;
        private readonly Mock<UserManager<AuctionUser>> mockedUserManager;

        private readonly DeleteAdminCommandHandler handler;

        public DeleteAdminCommandTests()
        {
            this.mockedUserManager = IdentityMocker.GetMockedUserManager();
            this.userManagerService = new UserManagerService(
                this.mockedUserManager.Object,
                IdentityMocker.GetMockedRoleManager().Object,
                this.Context);

            this.handler = new DeleteAdminCommandHandler(this.userManagerService);
        }

        [Fact]
        public async Task Handle_GivenValidModel_Should_Not_ThrowException()
        {
            this.mockedUserManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<AuctionUser>(), AppConstants.AdministratorRole))
                .ReturnsAsync(IdentityResult.Success);
            this.mockedUserManager
                .Setup(x => x.GetUsersInRoleAsync(AppConstants.AdministratorRole))
                .ReturnsAsync(new List<AuctionUser> { new AuctionUser { Id = Guid.NewGuid().ToString() } });

            var command = new DeleteAdminCommand { Email = "admin@admin.com", Role = "Administrator" };

            await this.handler.Handle(command, CancellationToken.None);
        }

        [Fact]
        public async Task Handle_GivenInvalidRole_Should_Throw_BadRequestException()
            => await Assert.ThrowsAsync<BadRequestException>(() =>
                this.handler.Handle(new DeleteAdminCommand()
                {
                    Email = "some random email",
                    Role = "invalid role"
                }, CancellationToken.None));

        [Fact]
        public async Task Handle_InCaseOfRemoveUserFromRoleFailure_Should_Throw_BadRequestException()
        {
            this.mockedUserManager
                .Setup(x => x.RemoveFromRoleAsync(It.IsAny<AuctionUser>(), AppConstants.AdministratorRole))
                .ReturnsAsync(IdentityResult.Success);
            this.mockedUserManager
                .Setup(x => x.GetUsersInRoleAsync(AppConstants.AdministratorRole))
                .ReturnsAsync(new List<AuctionUser>() { new AuctionUser { Id = DataConstants.SampleAdminUserId } });

            var command = new DeleteAdminCommand { Email = "admin@admin.com", Role = "Administrator" };

            await Assert.ThrowsAsync<BadRequestException>(() => this.handler.Handle(command, CancellationToken.None));
        }
    }
}
