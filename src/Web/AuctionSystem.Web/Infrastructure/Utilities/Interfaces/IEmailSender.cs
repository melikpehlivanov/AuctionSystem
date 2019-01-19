namespace AuctionSystem.Web.Infrastructure.Utilities.Interfaces
{
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task SendEmailAsync(string sender, string receiver, string subject, string htmlMessage);
    }
}
