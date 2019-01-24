namespace AuctionSystem.Worker.Runner
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.EmailSender.Interface;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models;

    public class JobManager
    {
        private const string AppMainEmailAddress = "mytestedauctionsystem@gmail.com";
        private const string CongratsMessage =
            "Congratulations {0}! You won bid for item - {1}. You will be contacted shortly by the seller for additional information.";

        private const string LogMessage = "Email was sent successfully on {0} utc time to {1} regarding his winning of item {2}";
        private const string ExceptionMessage = "Entity update failed. Exception message: {0}";
        private readonly AuctionSystemDbContext dbContext;
        private readonly IEmailSender emailSender;
        private readonly ILogger logger;

        public JobManager(AuctionSystemDbContext dbContext, IEmailSender emailSender, ILogger<JobManager> logger)
        {
            this.dbContext = dbContext;
            this.emailSender = emailSender;
            this.logger = logger;
        }

        public async Task ExecuteAllJobs(int repeatTimeInMinutes = 1)
        {
            // Add your Jobs/Tasks here
            await SendEmailToTheWinnersOfGivenBids();

            var millisecondsTimeOut = repeatTimeInMinutes * 60_000;
            Thread.Sleep(millisecondsTimeOut);
        }

        private async Task SendEmailToTheWinnersOfGivenBids()
        {
            var items = await this.dbContext.Items
                .Where(i => i.EndTime <= DateTime.UtcNow && !i.IsEmailSent && i.Bids.Count >= 1)
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    IsEmailSent = i.IsEmailSent,
                    WinnerAmount = i.Bids.Max(b => b.Amount),
                })
                .ToListAsync();

            foreach (var item in items)
            {
                var winnerBid = await this.dbContext.Bids
                    .Where(b => b.Amount == item.WinnerAmount && b.ItemId == item.Id)
                    .Select(b => new
                    {
                        UserEmail = b.User.Email,
                        UserFullName = b.User.FullName,
                    })
                    .SingleOrDefaultAsync();

                var isSuccessful = await this.emailSender.SendEmailAsync(AppMainEmailAddress, winnerBid.UserEmail, "You won a bid!",
                   string.Format(CongratsMessage, winnerBid.UserFullName, item.Title));

                try
                {
                    if (isSuccessful)
                    {
                        this.dbContext.Update(item);
                        await this.dbContext.SaveChangesAsync();
                        this.logger.LogInformation(string.Format(LogMessage, DateTime.UtcNow, winnerBid.UserEmail, item.Id));
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogInformation(string.Format(ExceptionMessage, ex.Message));
                }
            }
        }
    }
}