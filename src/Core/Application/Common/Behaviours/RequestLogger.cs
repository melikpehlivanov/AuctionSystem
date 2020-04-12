namespace Application.Common.Behaviours
{
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR.Pipeline;
    using Microsoft.Extensions.Logging;

    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;

        public RequestLogger(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            this.logger.LogInformation("AuctionSystem Request: {Name} {@UserId} {@Request}",
                name, this.currentUserService.UserId, request);

            return Task.CompletedTask;
        }
    }
}
