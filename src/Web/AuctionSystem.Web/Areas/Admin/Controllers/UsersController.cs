//namespace AuctionSystem.Web.Areas.Admin.Controllers
//{
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using AutoMapper;
//    using Domain.Entities;
//    using Microsoft.AspNetCore.Identity;
//    using Microsoft.AspNetCore.Mvc;
//    using Models.Users;

//    public class UsersController : AdminController
//    {
//        private readonly IMapper mapper;
//        private readonly UserManager<AuctionUser> userManager;
//        private readonly IUserService userService;

//        public UsersController(IMapper mapper, UserManager<AuctionUser> userManager, IUserService userService)
//        {
//            this.mapper = mapper;
//            this.userManager = userManager;
//            this.userService = userService;
//        }

//        public async Task<IActionResult> Index(int page = 1)
//        {
//            var users = (await this.userService.GetAllUsersAsync<UserListingServiceModel>())
//                .Select(this.mapper.Map<UserListingViewModel>)
//                .ToPaginatedList(page, WebConstants.UsersCountPerPage);
            
//            var adminIds = (await this.userManager
//                    .GetUsersInRoleAsync(WebConstants.AdministratorRole))
//                .Select(r => r.Id)
//                .ToHashSet();
            
//            foreach (var user in users)
//            {
//                var currentUserRoles = new List<string>();
//                var nonCurrentRoles = new List<string>();

//                if (adminIds.Contains(user.Id))
//                {
//                    currentUserRoles.Add(WebConstants.AdministratorRole);
//                }
//                else
//                {
//                    nonCurrentRoles.Add(WebConstants.AdministratorRole);
//                }

//                user.CurrentRoles = currentUserRoles;
//                user.NonCurrentRoles = nonCurrentRoles;
//            }

//            return this.View(users);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddToRole(string userEmail, string role)
//        {
//            if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(userEmail))
//            {
//                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
//                return this.RedirectToAction(nameof(this.Index));
//            }

//            var user = await this.userManager.FindByEmailAsync(userEmail);
//            if (user == null)
//            {
//                this.ShowErrorMessage(NotificationMessages.UserNotFound);
//                return this.RedirectToAction(nameof(this.Index));
//            }

//            var identityResult = await this.userManager.AddToRoleAsync(user, role);

//            var success = identityResult.Succeeded;
//            if (success)
//            {
//                this.ShowSuccessMessage(
//                    string.Format(NotificationMessages.UserAddedToRole, userEmail, role));
//            }
//            else
//            {
//                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
//            }

//            return this.RedirectToAction(nameof(this.Index));
//        }

//        [HttpPost]
//        public async Task<IActionResult> RemoveFromRole(string userEmail, string role)
//        {
//            if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(userEmail))
//            {
//                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
//                return this.RedirectToAction(nameof(this.Index));
//            }

//            var user = await this.userManager.FindByEmailAsync(userEmail);
//            if (user == null)
//            {
//                this.ShowErrorMessage(NotificationMessages.UserNotFound);
//                return this.RedirectToAction(nameof(this.Index));
//            }

//            var currentLoggedUser = await this.userManager.GetUserAsync(this.User);

//            if (user.Email == currentLoggedUser.Email)
//            {
//                this.ShowErrorMessage(string.Format(NotificationMessages.UnableToRemoveSelf, role));
//                return this.RedirectToAction(nameof(this.Index));
//            }

//            var identityResult = await this.userManager.RemoveFromRoleAsync(user, role);

//            var success = identityResult.Succeeded;
//            if (success)
//            {
//                this.ShowSuccessMessage(
//                    string.Format(NotificationMessages.UserRemovedFromRole, userEmail, role));
//            }
//            else
//            {
//                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
//            }

//            return this.RedirectToAction(nameof(this.Index));
//        }
//    }
//}
