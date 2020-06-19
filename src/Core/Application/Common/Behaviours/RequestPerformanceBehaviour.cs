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
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<TRequest> logger;

        private readonly Stopwatch timer;

        public RequestPerformanceBehaviour(ICurrentUserService currentUserService, ILogger<TRequest> logger)
        {
            this.currentUserService = currentUserService;
            this.logger = logger;
            this.timer = new Stopwatch();
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

            this.logger.LogWarning(
                "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) @userId {@UserId} {@Request}",
                name, this.timer.ElapsedMilliseconds, this.currentUserService.UserId, request);

            return response;
        }
    }
}