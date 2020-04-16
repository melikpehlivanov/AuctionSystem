namespace MvcWeb.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    public abstract class BaseController : Controller
    {
        private IMediator mediator;

        protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();

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