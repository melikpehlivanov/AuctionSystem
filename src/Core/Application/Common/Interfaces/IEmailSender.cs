namespace Application.Common.Interfaces
{
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string sender, string receiver, string subject, string htmlMessage);
    }
}
