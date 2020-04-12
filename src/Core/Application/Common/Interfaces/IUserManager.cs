namespace Application.Common.Interfaces
{
    using System.Threading.Tasks;
    using Models;

    public interface IUserManager
    {
        Task<User> GetUserByUsernameAsync(string id);

        Task<User> GetUserByIdAsync(string id);

        Task<string> GetUserUsernameByIdAsync(string id);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string fullName);

        Task<Result> DeleteUserAsync(string userId);

        Task<(Result Result, string UserId)> CheckCredentials(string email, string password);
    }
}
