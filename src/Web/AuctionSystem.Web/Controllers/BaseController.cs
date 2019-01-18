namespace AuctionSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public abstract class BaseController : Controller
    {
        protected void Error(string message)
        {
            this.TempData[WebConstants.TempDataErrorMessageKey] = message;
        }

        protected void Success(string message)
        {
            this.TempData[WebConstants.TempDataSuccessMessageKey] = message;
        }
    }
}