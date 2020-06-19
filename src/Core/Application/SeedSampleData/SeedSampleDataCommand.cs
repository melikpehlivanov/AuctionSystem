namespace Application.SeedSampleData
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using global::Common;
    using MediatR;

    public class SeedSampleDataCommand : IRequest { }

    public class SeedSampleDataCommandHandler : IRequestHandler<SeedSampleDataCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IDateTime dateTime;
        private readonly IUserManager userManager;

        public SeedSampleDataCommandHandler(IAuctionSystemDbContext context,
            IDateTime dateTime,
            IUserManager userManager)
        {
            this.context = context;
            this.dateTime = dateTime;
            this.userManager = userManager;
        }


        public async Task<Unit> Handle(SeedSampleDataCommand request, CancellationToken cancellationToken)
        {
            var seeder = new Seeder(this.context, this.dateTime, this.userManager);
            await seeder.SeedAllAsync(cancellationToken);
            return Unit.Value;
        }
    }
}