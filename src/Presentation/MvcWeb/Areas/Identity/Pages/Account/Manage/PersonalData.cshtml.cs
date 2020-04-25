namespace MvcWeb.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<AuctionUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        public PersonalDataModel(
            UserManager<AuctionUser> userManager,
            ILogger<PersonalDataModel> logger)
        {
            this._userManager = userManager;
            this._logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            return this.Page();
        }
    }
}