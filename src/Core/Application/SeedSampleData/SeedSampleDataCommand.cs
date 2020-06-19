namespace Application.SeedSampleData
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using global::Common;
    using MediatR;

    public class SeedSampleDataCommand : IRequest
    {
    }

    public class SeedSampleDataCommandHandler : IRequestHandler<SeedSampleDataCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IUserManager userManager;
        private readonly IDateTime dateTime;

        public SeedSampleDataCommandHandler(IAuctionSystemDbContext context, IUserManager userManager, IDateTime dateTime)
        {
            this.context = context;
            this.userManager = userManager;
            this.dateTime = dateTime;
        }

        public async Task<Unit> Handle(SeedSampleDataCommand request, CancellationToken cancellationToken)
        {
            var seeder = new Seeder(this.context, this.userManager, this.dateTime);
            await seeder.SeedAllAsync(cancellationToken);
            return Unit.Value;
        }
    }
}