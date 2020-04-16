namespace MvcWeb.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class Disable2faModel : PageModel
    {
        private readonly UserManager<AuctionUser> _userManager;
        private readonly ILogger<Disable2faModel> _logger;

        public Disable2faModel(
            UserManager<AuctionUser> userManager,
            ILogger<Disable2faModel> logger)
        {
            this._userManager = userManager;
            this._logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            if (!await this._userManager.GetTwoFactorEnabledAsync(user))
            {
                throw new InvalidOperationException($"Cannot disable 2FA for user with ID '{this._userManager.GetUserId(this.User)}' as it's not currently enabled.");
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

            var disable2faResult = await this._userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred disabling 2FA for user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            this._logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", this._userManager.GetUserId(this.User));
            this.StatusMessage = "2fa has been disabled. You can reenable 2fa when you setup an authenticator app";
            return this.RedirectToPage("./TwoFactorAuthentication");
        }
    }
}