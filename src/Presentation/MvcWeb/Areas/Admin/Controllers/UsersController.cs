namespace MvcWeb.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Admin.Commands.CreateAdmin;
    using Application.Admin.Commands.DeleteAdmin;
    using Application.Admin.Queries.List;
    using Application.Common.Exceptions;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Models.Users;

    public class UsersController : AdminController
    {
        private readonly IMapper mapper;

        public UsersController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            //TODO: add pagination
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
            catch (ValidationException)
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
                return this.RedirectToAction(nameof(this.Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userEmail, string role)
        {
            try
            {
                await this.Mediator.Send(new DeleteAdminCommand { Email = userEmail, Role = role });
                this.ShowSuccessMessage(
                    string.Format(NotificationMessages.UserRemovedFromRole, userEmail, role));
                return this.RedirectToAction(nameof(this.Index));
            }
            catch (BadRequestException ex)
            {
                this.ShowErrorMessage(ex.Message);
                return this.RedirectToAction(nameof(this.Index));
            }
            catch (ValidationException)
            {
                this.ShowErrorMessage(NotificationMessages.TryAgainLaterError);
                return this.RedirectToAction(nameof(this.Index));
            }
        }
    }
}
