namespace AuctionSystem.Infrastructure.Identity
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application;
    using Application.Common.Interfaces;
    using Application.Common.Models;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserManagerService : IUserManager
    {
        private readonly IAuctionSystemDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AuctionUser> userManager;

        public UserManagerService(
            UserManager<AuctionUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAuctionSystemDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var result = await this.context
                .Users
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();

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
                TwoFactorEnabled = result.TwoFactorEnabled
            };

            return user;
        }

        public async Task<Result> CreateUserAsync(string email, string password, string fullName)
        {
            var user = new AuctionUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

            var result = await this.userManager.CreateAsync(user, password);
            return result.ToApplicationResult();
        }

        public async Task<Result> CreateUserAsync(AuctionUser user, string password)
        {
            var result = await this.userManager.CreateAsync(user, password);
            return result.ToApplicationResult();
        }

        public async Task<(Result Result, string UserId)> SignIn(string email, string password)
        {
            var user = await this.GetDomainUserByEmailAsync(email);
            if (user == null)
            {
                return (Result.Failure(ExceptionMessages.User.InvalidCredentials), null);
            }

            if (await this.userManager.IsLockedOutAsync(user))
            {
                return (
                    Result.Failure(
                        ExceptionMessages.User.AccountLockout), null);
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, password);
            if (!passwordValid)
            {
                await this.userManager.AccessFailedAsync(user);
                return (Result.Failure(ExceptionMessages.User.InvalidCredentials), null);
            }

            if (!await this.userManager.IsEmailConfirmedAsync(user))
            {
                return (
                    Result.Failure(ExceptionMessages.User.ConfirmAccount,
                        ErrorType.TokenExpired), null);
            }

            return (Result.Success(), user.Id);
        }

        public async Task CreateRoleAsync(IdentityRole role)
        {
            var roleExist = await this.roleManager.RoleExistsAsync(AppConstants.AdministratorRole);

            if (!roleExist)
            {
                await this.roleManager.CreateAsync(new IdentityRole(AppConstants.AdministratorRole));
            }
        }

        public async Task AddToRoleAsync(AuctionUser user, string role) =>
            await this.userManager.AddToRoleAsync(user, role);

        public async Task<Result> AddToRoleAsync(string email, string role, string currentUserId)
        {
            var user = await this.GetDomainUserByEmailAsync(email);

            if (user == null)
            {
                return Result.Failure(
                    string.Format(ExceptionMessages.Admin.UserNotAddedSuccessfullyToRole, role));
            }

            // This "admin" has no permission
            var refreshToken =
                await this.GetLastValidToken(currentUserId, CancellationToken.None);
            if (refreshToken == null)
            {
                return Result.Failure(
                    string.Format(ExceptionMessages.Admin.UserNotAddedSuccessfullyToRole, role),
                    ErrorType.TokenExpired);
            }

            var result = await this.userManager.AddToRoleAsync(user, role);
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(
                    string.Format(ExceptionMessages.Admin.UserNotAddedSuccessfullyToRole, role));
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await this.context
                .Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            var userRoles = await this.userManager.GetRolesAsync(user);
            return userRoles;
        }

        public async Task<string> GetFirstUserId()
        {
            var user = await this.context.Users.FirstAsync();
            return user.Id;
        }

        public async Task<IEnumerable<string>> GetUsersInRoleAsync(string role)
        {
            var users = await this.userManager.GetUsersInRoleAsync(role);
            return users.Select(r => r.Id).ToList();
        }

        public async Task<Result> RemoveFromRoleAsync(
            string email,
            string role,
            string currentUserId,
            CancellationToken cancellationToken)
        {
            var user = await this.GetDomainUserByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure(ExceptionMessages.User.UserNotFound);
            }

            var administrators = await this.GetUsersInRoleAsync(role);
            if (administrators.Contains(user.Id) && currentUserId == user.Id)
            {
                return Result.Failure(
                    string.Format(ExceptionMessages.Admin.CannotRemoveSelfFromRole, role));
            }

            if (!administrators.Contains(user.Id))
            {
                return Result.Failure(
                    string.Format(ExceptionMessages.Admin.NotInRole, user.Email, role));
            }

            var refreshToken =
                await this.GetLastValidToken(currentUserId, cancellationToken);
            // This "admin" has no permission
            if (refreshToken == null)
            {
                return Result.Failure(
                    string.Format(ExceptionMessages.Admin.UserNotRemovedSuccessfullyFromRole, role),
                    ErrorType.TokenExpired);
            }

            var result = await this.userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return Result.Failure(
                    string.Format(ExceptionMessages.Admin.UserNotRemovedSuccessfullyFromRole, role));
            }

            // Invalidate demoted user refresh token
            var removedUserRefreshToken =
                await this.GetLastValidToken(user.Id, cancellationToken);
            removedUserRefreshToken.Invalidated = true;
            await this.context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<string> GenerateEmailConfirmationCode(string email)
        {
            var user = await this.GetDomainUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var token = await this.userManager.GenerateUserTokenAsync(user, FourDigitTokenProvider.FourDigitEmail,
                "Confirmation");
            return token;
        }

        public async Task<bool> ConfirmEmail(string email,
            string token)
        {
            var user = await this.GetDomainUserByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await this.userManager.VerifyUserTokenAsync(user, FourDigitTokenProvider.FourDigitEmail,
                "Confirmation", token);
            user.EmailConfirmed = true;
            this.context.Users.Update(user);
            await this.context.SaveChangesAsync(CancellationToken.None);
            return result;
        }

        private async Task<AuctionUser> GetDomainUserByEmailAsync(string email)
            => await this.context.Users.Where(u => u.Email == email).SingleOrDefaultAsync();

        private async Task<RefreshToken> GetLastValidToken(string currentUserId, CancellationToken cancellationToken)
            => await this.context.RefreshTokens.SingleOrDefaultAsync(
                x => x.UserId == currentUserId && !x.Invalidated,
                cancellationToken);
    }
}