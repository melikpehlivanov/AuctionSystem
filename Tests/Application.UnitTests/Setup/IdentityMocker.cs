namespace Application.UnitTests.Setup
{
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class IdentityMocker
    {
        public static Mock<UserManager<AuctionUser>> GetMockedUserManager()
        {
            var userStoreMock = new Mock<IUserStore<AuctionUser>>();
            return new Mock<UserManager<AuctionUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        public static Mock<RoleManager<IdentityRole>> GetMockedRoleManager()
        {
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            var roleManager = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);
            return roleManager;
        }
    }
}