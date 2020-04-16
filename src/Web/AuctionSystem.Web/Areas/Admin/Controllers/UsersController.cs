namespace AuctionSystem.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Admin.Queries.List;
    using AutoMapper;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Users;

    public class UsersController : AdminController
    {
        private readonly IMapper mapper;
        private readonly UserManager<AuctionUser> userManager;

        public UsersController(IMapper mapper, UserManager<AuctionUser> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var response = await this.Mediator.Send(new ListAllUsersQuery());
            var users = response.Data
                .Select(this.mapper.Map<UserListingViewModel>)
                .ToList();

            return this.View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(string userEmail, string role)
        {
            if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(userEmail))
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
                return this.RedirectToAction(nameof(this.Index));
            }

            var user = await this.userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                this.ShowErrorMessage(NotificationMessages.UserNotFound);
                return this.RedirectToAction(nameof(this.Index));
            }

            var identityResult = await this.userManager.AddToRoleAsync(user, role);

            var success = identityResult.Succeeded;
            if (success)
            {
                this.ShowSuccessMessage(
                    string.Format(NotificationMessages.UserAddedToRole, userEmail, role));
            }
            else
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userEmail, string role)
        {
            if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(userEmail))
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
                return this.RedirectToAction(nameof(this.Index));
            }

            var user = await this.userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                this.ShowErrorMessage(NotificationMessages.UserNotFound);
                return this.RedirectToAction(nameof(this.Index));
            }

            var currentLoggedUser = await this.userManager.GetUserAsync(this.User);

            if (user.Email == currentLoggedUser.Email)
            {
                this.ShowErrorMessage(string.Format(NotificationMessages.UnableToRemoveSelf, role));
                return this.RedirectToAction(nameof(this.Index));
            }

            var identityResult = await this.userManager.RemoveFromRoleAsync(user, role);

            var success = identityResult.Succeeded;
            if (success)
            {
                this.ShowSuccessMessage(
                    string.Format(NotificationMessages.UserRemovedFromRole, userEmail, role));
            }
            else
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
