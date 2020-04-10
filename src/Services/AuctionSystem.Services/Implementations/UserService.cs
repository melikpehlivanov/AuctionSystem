namespace AuctionSystem.Services.Implementations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class UserService : BaseService, IUserService
    {
        public UserService(IMapper mapper, AuctionSystemDbContext context) 
            : base(mapper, context)
        {
        }

        public async Task<string> GetUserIdByUsernameAsync(string username)
        {
            var user = await this.Context
                .Users
                .SingleOrDefaultAsync(u => u.UserName == username);

            return user?.Id;
        }

        public async Task<IEnumerable<T>> GetAllUsersAsync<T>()
            => await this.Context
                .Users
                .ProjectTo<T>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
