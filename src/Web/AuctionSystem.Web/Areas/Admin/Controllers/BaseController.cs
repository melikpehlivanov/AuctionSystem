namespace AuctionSystem.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        protected void ShowErrorMessage(string message)
        {
            this.TempData[WebConstants.TempDataErrorMessageKey] = message;
        }

        protected void ShowSuccessMessage(string message)
        {
            this.TempData[WebConstants.TempDataSuccessMessageKey] = message;
        }

        protected IActionResult RedirectToHome() => this.Redirect("/");
    }
}
