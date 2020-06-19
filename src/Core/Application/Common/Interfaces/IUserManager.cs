namespace Application.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Models;

    public interface IUserManager
    {
        Task<AuctionUser> GetDomainUserByUsernameAsync(string username);

        Task<User> GetUserByUsernameAsync(string id);

        Task<User> GetUserByIdAsync(string id);

        Task<string> GetUserUsernameByIdAsync(string username);

        Task<Result> CreateUserAsync(string userName, string password, string fullName);

        Task<Result> CreateUserAsync(AuctionUser user, string password);

        Task<Result> DeleteUserAsync(string userId);

        Task<(Result Result, string UserId)> CheckCredentials(string email, string password);

        Task CreateRoleAsync(IdentityRole role);

        Task AddToRoleAsync(AuctionUser user, string role);

        Task<IdentityResult> AddToRoleAsync(string username, string role);

        Task<IList<string>> GetUserRolesAsync(string userId);

        Task<string> GetFirstUserId();

        Task<string> GetUserIdByUsernameAsync(string username);

        Task<IEnumerable<string>> GetUsersInRoleAsync(string role);

        //TODO: Extract in model
        Task<(IdentityResult identityResult, string errorMessage)> RemoveFromRoleAsync(
            string username,
            string role,
            string currentUserId);
    }
}