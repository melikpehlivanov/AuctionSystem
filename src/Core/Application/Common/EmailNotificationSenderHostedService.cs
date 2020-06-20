namespace Application.Common
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application;
    using global::Common;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class EmailNotificationSenderHostedService : IHostedService, IDisposable
    {
        private const string CongratsMessage =
            "Congratulations {0}! You won bid for item - {1}. You will be contacted shortly by the seller for additional information.";

        private const string CongratsMessageForItemSeller =
            "Congratulations {0}! Your item - {1} - was sucessfully sold for €{2}. Please contact the buyer to arrange the shipping and etc. Buyer email - {3}.";

        private const string LogMessage =
            "Email was sent successfully on {0} utc time to {1} regarding the winning of item {2}";

        private const string LogMessageForItemOwner =
            "Email was sent successfully on {0} utc time to {1} regarding of {2} being sold";

        private const string ExceptionMessage = "Entity update failed. Exception message: {0}";
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IDateTime dateTime;
        private readonly ILogger logger;
        private readonly IEmailSender emailSender;
        private Timer timer;

        public EmailNotificationSenderHostedService(IServiceScopeFactory scopeFactory,
            IDateTime dateTime,
            ILogger<EmailNotificationSenderHostedService> logger,
            IEmailSender emailSender)
        {
            this.scopeFactory = scopeFactory;
            this.dateTime = dateTime;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Email notification Background Service is starting.");

            this.timer = new Timer(this.DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            this.logger.LogInformation("Email notification Background Service is working.");
            await this.SendEmailToTheWinnersOfGivenBids();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Email notification Background Service is stopping.");

            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }

        private async Task SendEmailToTheWinnersOfGivenBids()
        {
            using var scope = this.scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IAuctionSystemDbContext>();
            var items = await context.Items
                .Where(i => i.EndTime <= this.dateTime.UtcNow && !i.IsEmailSent && i.Bids.Count >= 1)
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    IsEmailSent = i.IsEmailSent,
                    WinnerAmount = i.Bids.Max(b => b.Amount),
                    UserEmail = i.User.Email,
                    UserFullName = i.User.FullName
                })
                .ToListAsync();

            foreach (var item in items)
            {
                var winnerBid = await context.Bids
                    .Where(b => b.Amount == item.WinnerAmount && b.ItemId == item.Id)
                    .Select(b => new
                    {
                        UserEmail = b.User.Email,
                        UserFullName = b.User.FullName,
                        b.Amount
                    })
                    .SingleOrDefaultAsync();

                var emailSendToItemOwner = await this.emailSender.SendEmailAsync(AppConstants.AppMainEmailAddress,
                    item.UserEmail,
                    "Your item was sold!",
                    string.Format(CongratsMessageForItemSeller, item.UserFullName, item.Title, winnerBid.Amount,
                        winnerBid.UserEmail));
                var successful = await this.emailSender.SendEmailAsync(AppConstants.AppMainEmailAddress,
                    winnerBid.UserEmail,
                    "You won a bid!",
                    string.Format(CongratsMessage, winnerBid.UserFullName, item.Title));

                if (!successful || !emailSendToItemOwner)
                {
                    continue;
                }

                try
                {
                    var dbItem = await context.Items.FindAsync(item.Id);
                    if (dbItem == null)
                    {
                        continue;
                    }

                    dbItem.IsEmailSent = true;
                    context.Items.Update(dbItem);
                    await context.SaveChangesAsync(CancellationToken.None);
                    this.logger.LogInformation(string.Format(LogMessage, this.dateTime.UtcNow, winnerBid.UserEmail,
                        item.Title));
                    this.logger.LogInformation(string.Format(LogMessageForItemOwner, this.dateTime.UtcNow,
                        item.UserEmail,
                        item.Title));
                }
                catch (Exception ex)
                {
                    this.logger.LogInformation(string.Format(ExceptionMessage, ex.Message));
                }
            }
        }

        private class ItemDto
        {
            public Guid Id { get; set; }

            public string Title { get; set; }

            public bool IsEmailSent { get; set; }

            public decimal WinnerAmount { get; set; }

            public string UserEmail { get; set; }

            public string UserFullName { get; set; }
        }
    }
}