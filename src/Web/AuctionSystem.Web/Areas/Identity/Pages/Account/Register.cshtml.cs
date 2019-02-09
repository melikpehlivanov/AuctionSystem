namespace AuctionSystem.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using Models;
    using IEmailSender = Common.EmailSender.Interface.IEmailSender;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<AuctionUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AuctionUser> userManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [MaxLength(50)]
            [Display(Name = "Full name")]
            public string FullName { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToHome();
            }

            ReturnUrl = returnUrl;
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToHome();
            }

            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new AuctionUser { UserName = this.Input.Email, Email = this.Input.Email, FullName = this.Input.FullName};
                var result = await _userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(WebConstants.AppMainEmailAddress, Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    this.TempData[WebConstants.TempDataSuccessMessageKey] = NotificationMessages.ActivateYourAccountMessage;
                    return this.RedirectToHome();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IActionResult RedirectToHome()
            => this.RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
    }
}
