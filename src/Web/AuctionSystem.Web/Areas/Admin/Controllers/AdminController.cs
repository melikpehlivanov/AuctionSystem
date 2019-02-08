namespace AuctionSystem.Web.Areas.Admin.Controllers
{
    using AuctionSystem.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    [Authorize(Roles = WebConstants.AdministratorRole)]
    public abstract class AdminController : BaseController
    {
    }
}
