namespace Application.Common
{
    using System.Threading.Tasks;
    using Application;
    using Interfaces;

    public static class EmailSenderHelper
    {
        public static async Task SendConfirmationEmail(this IEmailSender emailSender, string email, string token)
        {
            await emailSender.SendEmailAsync(AppConstants.AppMainEmailAddress, email, "Verification code",
                $"Thanks for registering. Your verification code is <b>{token}</b>.");
        }
    }
}