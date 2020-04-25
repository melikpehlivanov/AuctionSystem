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
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<AuctionUser> _signInManager;
        private readonly ILogger<LoginWith2faModel> _logger;

        public LoginWith2faModel(SignInManager<AuctionUser> signInManager, ILogger<LoginWith2faModel> logger)
        {
            this._signInManager = signInManager;
            this._logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await this._signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            this.ReturnUrl = returnUrl;
            this.RememberMe = rememberMe;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            var user = await this._signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = this.Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await this._signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, this.Input.RememberMachine);

            if (result.Succeeded)
            {
                this._logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return this.LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                this._logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                this._logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                this.ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return this.Page();
            }
        }
    }
}
