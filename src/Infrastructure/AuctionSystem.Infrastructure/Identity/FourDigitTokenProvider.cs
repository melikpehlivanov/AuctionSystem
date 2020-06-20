namespace AuctionSystem.Infrastructure.Identity
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;

    public class FourDigitTokenProvider : PhoneNumberTokenProvider<AuctionUser>
    {
        public const string FourDigitPhone = "4DigitPhone";
        public const string FourDigitEmail = "4DigitEmail";

        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AuctionUser> manager, AuctionUser user) =>
            Task.FromResult(false);

        public override async Task<string> GenerateAsync(string purpose,
            UserManager<AuctionUser> manager,
            AuctionUser user)
        {
            var token = new SecurityToken(await manager.CreateSecurityTokenAsync(user));
            var modifier = await this.GetUserModifierAsync(purpose, manager, user);
            var code = Rfc6238AuthenticationService.GenerateCode(token, modifier, 4)
                .ToString("D4", CultureInfo.InvariantCulture);

            return code;
        }

        public override async Task<bool> ValidateAsync(string purpose,
            string token,
            UserManager<AuctionUser> manager,
            AuctionUser user)
        {
            if (!int.TryParse(token, out var code))
            {
                return false;
            }

            var securityToken = new SecurityToken(await manager.CreateSecurityTokenAsync(user));
            var modifier = await this.GetUserModifierAsync(purpose, manager, user);
            var valid = Rfc6238AuthenticationService.ValidateCode(securityToken, code, modifier, token.Length);
            return valid;
        }

        public override Task<string> GetUserModifierAsync(string purpose,
            UserManager<AuctionUser> manager,
            AuctionUser user) => base.GetUserModifierAsync(purpose, manager, user);
    }
}