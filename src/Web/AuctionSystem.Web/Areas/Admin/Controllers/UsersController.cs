namespace AuctionSystem.Web.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Admin.Commands.CreateAdmin;
    using Application.Admin.Queries.List;
    using Application.Common.Exceptions;
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
            try
            {
                await this.Mediator.Send(new CreateAdminCommand { Email = userEmail, Role = role });
                this.ShowSuccessMessage(
                    string.Format(NotificationMessages.UserAddedToRole, userEmail, role));
                return this.RedirectToAction(nameof(this.Index));
            }
            catch (BadRequestException ex)
            {
                this.ShowErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(this.Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userEmail, string role)
        {
            try
            {
                //await this.Mediator.Send(new AddUserToRoleCommand { Email = userEmail, Role = role });
                this.ShowSuccessMessage(
                    string.Format(NotificationMessages.UserRemovedFromRole, userEmail, role));
                return this.RedirectToAction(nameof(this.Index));
            }
            catch (BadRequestException ex)
            {
                this.ShowErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(this.Index));
            }
        }
    }
}
