namespace MvcWeb.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class GenerateRecoveryCodesModel : PageModel
    {
        private readonly UserManager<AuctionUser> _userManager;
        private readonly ILogger<GenerateRecoveryCodesModel> _logger;

        public GenerateRecoveryCodesModel(
            UserManager<AuctionUser> userManager,
            ILogger<GenerateRecoveryCodesModel> logger)
        {
            this._userManager = userManager;
            this._logger = logger;
        }

        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            var isTwoFactorEnabled = await this._userManager.GetTwoFactorEnabledAsync(user);
            if (!isTwoFactorEnabled)
            {
                var userId = await this._userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{userId}' because they do not have 2FA enabled.");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            var isTwoFactorEnabled = await this._userManager.GetTwoFactorEnabledAsync(user);
            var userId = await this._userManager.GetUserIdAsync(user);
            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException($"Cannot generate recovery codes for user with ID '{userId}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await this._userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            this.RecoveryCodes = recoveryCodes.ToArray();

            this._logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);
            this.StatusMessage = "You have generated new recovery codes.";
            return this.RedirectToPage("./ShowRecoveryCodes");
        }
    }
}