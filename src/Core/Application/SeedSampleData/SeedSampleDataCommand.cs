namespace Application.SeedSampleData
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using MediatR;

    public class SeedSampleDataCommand : IRequest
    {
    }

    public class SeedSampleDataCommandHandler : IRequestHandler<SeedSampleDataCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IUserManager userManager;

        public SeedSampleDataCommandHandler(IAuctionSystemDbContext context, IUserManager userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(SeedSampleDataCommand request, CancellationToken cancellationToken)
        {
            var seeder = new Seeder(this.context, this.userManager);
            await seeder.SeedAllAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
