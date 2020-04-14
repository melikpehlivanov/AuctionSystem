namespace Application.Common.Interfaces
{
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Models;

    public interface IUserManager
    {
        Task<User> GetUserByUsernameAsync(string id);

        Task<User> GetUserByIdAsync(string id);

        Task<string> GetUserUsernameByIdAsync(string id);

        Task<Result> CreateUserAsync(string userName, string password, string fullName);

        Task<Result> CreateUserAsync(AuctionUser user, string password);

        Task<Result> DeleteUserAsync(string userId);

        Task<(Result Result, string UserId)> CheckCredentials(string email, string password);
        Task CreateRoleAsync(IdentityRole role);

        Task AddToRoleAsync(AuctionUser user, string role);

        Task<string> GetFirstUserId();
        Task<string> GetUserIdByUsernameAsync(string username);
    }
}
