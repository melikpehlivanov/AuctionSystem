namespace MvcWeb.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Application;
    using Application.Common.Interfaces;
    using Domain.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<AuctionUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<AuctionUser> userManager, IEmailSender emailSender)
        {
            this._userManager = userManager;
            this._emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (this.ModelState.IsValid)
            {
                var user = await this._userManager.FindByEmailAsync(this.Input.Email);
                if (user == null || !(await this._userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return this.RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await this._userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = this.Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code, user.Email },
                    protocol: this.Request.Scheme);

                await this._emailSender.SendEmailAsync(
                    AppConstants.AppMainEmailAddress,
                    this.Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return this.RedirectToPage("./ForgotPasswordConfirmation");
            }

            return this.Page();
        }
    }
}
