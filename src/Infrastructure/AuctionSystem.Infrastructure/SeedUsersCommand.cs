namespace AuctionSystem.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application;
    using Application.Common.Interfaces;
    using Application.Common.Models;
    using Identity;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class SeedUsersCommand : IRequest
    {
    }

    public class SeedUsersCommandHandler : IRequestHandler<SeedUsersCommand>
    {
        private readonly IUserManager userManager;
        private readonly ApplicationDbContext dbContext;

        public SeedUsersCommandHandler(IUserManager userManager, ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(SeedUsersCommand request, CancellationToken cancellationToken)
        {
            await this.SeedAdminRole();
            await this.SeedUsers();

            return Unit.Value;
        }

        private async Task SeedUsers()
        {
            if (!await this.dbContext.Users.AnyAsync())
            {
                var allUsers = new List<ApplicationUser>();
                for (int i = 1; i <= 2; i++)
                {
                    var user = new ApplicationUser
                    {
                        Email = $"test{i}@test.com",
                        FullName = $"Test Testov{i}",
                        UserName = $"test{i}@test.com",
                        EmailConfirmed = true
                    };

                    allUsers.Add(user);
                }

                foreach (var user in allUsers)
                {
                    await this.userManager.CreateUserAsync(user, "test123");
                }

                var admin = new ApplicationUser
                {
                    Email = "admin@admin.com",
                    FullName = "Admin Adminski",
                    UserName = "admin@admin.com",
                    EmailConfirmed = true
                };

                await this.userManager.CreateUserAsync(admin, "admin123");
                await this.userManager.AddToRoleAsync(admin, AppConstants.AdministratorRole);
            }
        }

        private async Task SeedAdminRole()
            => await this.userManager.CreateRoleAsync(new IdentityRole(AppConstants.AdministratorRole));
    }
}
