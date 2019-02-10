namespace AuctionSystem.Web.Controllers
{
    using System.Threading.Tasks;
    using Common.EmailSender.Interface;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Contact;

    public class ContactController : BaseController
    {
        private readonly IEmailSender emailSender;

        public ContactController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public IActionResult Index() => this.PartialView("_ContactUsPartial");

        public async Task<IActionResult> SendMessage(ContactUsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_ContactUsPartial", model);
            }

            await this.emailSender.SendEmailAsync(model.Email, WebConstants.AppMainEmailAddress, model.Subject,
                model.Message);

            this.ShowSuccessMessage(NotificationMessages.EmailSentSuccessfully);
            return this.RedirectToHome();
        }
    }
}
