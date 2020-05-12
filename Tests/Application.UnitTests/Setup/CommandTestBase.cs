namespace Application.UnitTests.Setup
{
    using System;
    using AutoMapper;
    using Persistence;

    public class CommandTestBase : IDisposable
    {
        public CommandTestBase()
        {
            this.Context = AuctionSystemContextFactory.Create();

            this.Mapper = TestSetup.InitializeMapper();
        }

        protected AuctionSystemDbContext Context { get; }

        protected IMapper Mapper { get; }

        public void Dispose()
            => AuctionSystemContextFactory.Destroy(this.Context);
    }
}