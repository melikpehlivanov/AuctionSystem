namespace AuctionSystem.Common.EmailSender.Implementation
{
    using System.Net;
    using System.Threading.Tasks;
    using Interface;
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSender : IEmailSender
    {
        private readonly SendGridOptions options;

        public EmailSender(IOptions<SendGridOptions> options)
        {
            this.options = options.Value;
        }

        public async Task<bool> SendEmailAsync(string sender, string receiver, string subject, string htmlMessage)
        {
            var client = new SendGridClient(this.options.SendGridApiKey);
            var from = new EmailAddress(sender);
            var to = new EmailAddress(receiver, receiver);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlMessage, htmlMessage);
            var isSuccessful = await client.SendEmailAsync(msg);

            return isSuccessful.StatusCode == HttpStatusCode.Accepted;
        }
    }
}
