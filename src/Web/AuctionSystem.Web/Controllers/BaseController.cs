namespace AuctionSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public abstract class BaseController : Controller
    {
        protected void ShowErrorMessage(string message)
        {
            this.TempData[WebConstants.TempDataErrorMessageKey] = message;
        }

        protected void ShowSuccessMessage(string message)
        {
            this.TempData[WebConstants.TempDataSuccessMessageKey] = message;
        }
    }
}