namespace MvcWeb.Areas.Identity.Pages.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class LoginWithRecoveryCodeModel : PageModel
    {
        private readonly SignInManager<AuctionUser> _signInManager;
        private readonly ILogger<LoginWithRecoveryCodeModel> _logger;

        public LoginWithRecoveryCodeModel(SignInManager<AuctionUser> signInManager, ILogger<LoginWithRecoveryCodeModel> logger)
        {
            this._signInManager = signInManager;
            this._logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [BindProperty]
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await this._signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            this.ReturnUrl = returnUrl;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this._signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = this.Input.RecoveryCode.Replace(" ", string.Empty);

            var result = await this._signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                this._logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
                return this.LocalRedirect(returnUrl ?? this.Url.Content("~/"));
            }
            if (result.IsLockedOut)
            {
                this._logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                this._logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
                this.ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return this.Page();
            }
        }
    }
}
