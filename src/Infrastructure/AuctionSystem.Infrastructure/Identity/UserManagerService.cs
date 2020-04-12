namespace AuctionSystem.Infrastructure.Identity
{
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Application.Common.Models;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;

    public class UserManagerService : IUserManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserManagerService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var result = await this.userManager.FindByNameAsync(username);
            var user = this.mapper.Map<User>(result);

            return user;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var result = await this.userManager.FindByIdAsync(id);
            var user = this.mapper.Map<User>(result);

            return user;
        }

        public async Task<string> GetUserUsernameByIdAsync(string id)
        {
            var result = await this.userManager.FindByIdAsync(id);

            return result.UserName;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await this.userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
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


        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await this.userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}
