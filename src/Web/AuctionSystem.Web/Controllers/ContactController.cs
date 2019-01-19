namespace AuctionSystem.Web.Controllers
{
    using System.Threading.Tasks;
    using Infrastructure.Utilities.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Home;

    public class ContactController : BaseController
    {
        private readonly IEmailSender emailSender;

        public ContactController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        [HttpGet("/Home/Contact")]
        public IActionResult ContactUs() => this.PartialView("_ContactUsPartial");

        public async Task<IActionResult> OnPostAsync(ContactUsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_ContactUsPartial", model);
            }

            await this.emailSender.SendEmailAsync(model.Email, WebConstants.AppMainEmailAddress, model.Subject,
                model.Message);

            this.ShowSuccessMessage(WebConstants.EmailSentSuccessfully);
            return this.RedirectToHome();
        }
    }
}
