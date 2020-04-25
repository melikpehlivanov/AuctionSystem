namespace Application.Common.Behaviours
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private const int MaxResponseTime = 500;

        private readonly Stopwatch timer;
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<TRequest> logger;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            this.timer = new Stopwatch();

            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            this.timer.Start();

            var response = await next();

            this.timer.Stop();

            if (this.timer.ElapsedMilliseconds <= MaxResponseTime)
            {
                return response;
            }

            var name = typeof(TRequest).Name;

            this.logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) @userId {@UserId} {@Request}",
                name, this.timer.ElapsedMilliseconds, this.currentUserService.UserId, request);

            return response;
        }
    }
}