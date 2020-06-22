namespace Application.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Models;

    public interface IUserManager
    {
        Task<User> GetUserByIdAsync(string id);

        Task<Result> CreateUserAsync(string email, string password, string fullName);

        Task<Result> CreateUserAsync(AuctionUser user, string password);

        Task<(Result Result, string UserId)> SignIn(string email, string password);

        Task CreateRoleAsync(IdentityRole role);

        Task AddToRoleAsync(AuctionUser user, string role);

        Task<Result> AddToRoleAsync(string email, string role, string currentUserId);

        Task<IList<string>> GetUserRolesAsync(string userId);

        Task<string> GetFirstUserId();

        Task<IEnumerable<string>> GetUsersInRoleAsync(string role);

        Task<Result> RemoveFromRoleAsync(
            string username,
            string role,
            string currentUserId,
            CancellationToken cancellationToken);

        Task<string> GenerateEmailConfirmationCode(string email);

        Task<bool> ConfirmEmail(string email, string code);
    }
}