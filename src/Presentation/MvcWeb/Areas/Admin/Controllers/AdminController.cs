namespace MvcWeb.Areas.Admin.Controllers
{
    using Application;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MvcWeb.Controllers;

    [Area("Admin")]
    [Authorize(Roles = AppConstants.AdministratorRole)]
    public abstract class AdminController : BaseController
    {
    }
}
