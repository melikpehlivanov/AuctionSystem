namespace AuctionSystem.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<string> GetUserIdByUsernameAsync(string username);
    }
}
