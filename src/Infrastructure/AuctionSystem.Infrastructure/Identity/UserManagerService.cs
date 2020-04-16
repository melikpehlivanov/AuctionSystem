namespace AuctionSystem.Infrastructure.Identity
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application;
    using Application.Common.Interfaces;
    using Application.Common.Models;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserManagerService : IUserManager
    {
        private readonly UserManager<AuctionUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAuctionSystemDbContext context;

        public UserManagerService(UserManager<AuctionUser> userManager, RoleManager<IdentityRole> roleManager, IAuctionSystemDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var result = await this.userManager.FindByNameAsync(username);
            if (result == null)
            {
                return null;
            }

            var user = new User
            {
                Id = result.Id,
                Email = result.Email,
                UserName = result.UserName,
                FullName = result.FullName,
                AccessFailedCount = result.AccessFailedCount,
                IsEmailConfirmed = result.EmailConfirmed,
                LockoutEnd = result.LockoutEnd,
                PhoneNumber = result.PhoneNumber,
                PhoneNumberConfirmed = result.PhoneNumberConfirmed,
                TwoFactorEnabled = result.TwoFactorEnabled,
            };

            return user;
        }

        public async Task<AuctionUser> GetDomainUserByUsernameAsync(string username)
            => await this.userManager.FindByNameAsync(username);

        public async Task<User> GetUserByIdAsync(string id)
        {
            var result = await this.userManager.FindByIdAsync(id);
            if (result == null)
            {
                return null;
            }

            var user = new User
            {
                Id = result.Id,
                Email = result.Email,
                UserName = result.UserName,
                FullName = result.FullName,
                AccessFailedCount = result.AccessFailedCount,
                IsEmailConfirmed = result.EmailConfirmed,
                LockoutEnd = result.LockoutEnd,
                PhoneNumber = result.PhoneNumber,
                PhoneNumberConfirmed = result.PhoneNumberConfirmed,
                TwoFactorEnabled = result.TwoFactorEnabled,
            };

            return user;
        }

        public async Task<string> GetUserUsernameByIdAsync(string id)
        {
            var result = await this.userManager.FindByIdAsync(id);

            return result.UserName;
        }

        public async Task<Result> CreateUserAsync(string userName, string password, string fullName)
        {
            var user = new AuctionUser
            {
                UserName = userName,
                Email = userName,
                FullName = fullName,
            };

            var result = await this.userManager.CreateAsync(user, password);
            return result.ToApplicationResult();
        }

        public async Task<Result> CreateUserAsync(AuctionUser user, string password)
        {
            var result = await this.userManager.CreateAsync(user, password);
            return result.ToApplicationResult();
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = this.userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await this.DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<(Result Result, string UserId)> CheckCredentials(string email, string password)
        {
            var user = await this.userManager.FindByNameAsync(email);
            if (user == null)
            {
                return (Result.Failure(new List<string> { "User not found" }), null);
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, password);
            return !passwordValid
                ? (Result.Failure(new List<string> { "Wrong password" }), null)
                : (Result.Success(), user.Id);
        }

        public async Task CreateRoleAsync(IdentityRole role)
        {
            var roleExist = await this.roleManager.RoleExistsAsync(AppConstants.AdministratorRole);

            if (!roleExist)
            {
                await this.roleManager.CreateAsync(new IdentityRole(AppConstants.AdministratorRole));
            }
        }

        public async Task AddToRoleAsync(AuctionUser user, string role)
            => await this.userManager.AddToRoleAsync(user, role);

        public async Task<bool> AddToRoleAsync(string username, string role)
        {
            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                return false;
            }

            var result = await this.userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var userRoles = await this.userManager.GetRolesAsync(user);

            return userRoles;
        }

        public async Task<string> GetFirstUserId()
        {
            var user = await this.userManager.Users.FirstAsync();
            return user.Id;
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            return user?.Id;
        }

        public async Task<IEnumerable<string>> GetUsersInRoleAsync(string role)
        {
            var users = await this.userManager.GetUsersInRoleAsync(role);
            return users.Select(r => r.Id).ToList();
        }

        public async Task<Result> DeleteUserAsync(AuctionUser user)
        {
            var result = await this.userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}
