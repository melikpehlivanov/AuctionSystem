namespace AuctionSystem.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<string> GetUserIdByUsernameAsync(string username);

        Task<IEnumerable<T>> GetAllUsersAsync<T>();
    }
}
