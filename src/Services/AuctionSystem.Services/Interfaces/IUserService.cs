namespace AuctionSystem.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.AuctionUser;

    public interface IUserService
    {
        Task<string> GetUserIdByUsernameAsync(string username);

        Task<IEnumerable<UserListingServiceModel>> GetAllUsersAsync();
    }
}
