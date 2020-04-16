namespace MvcWeb.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MvcWeb.Controllers;

    [Area("Admin")]
    [Authorize(Roles = WebConstants.AdministratorRole)]
    public abstract class AdminController : BaseController
    {
    }
}
