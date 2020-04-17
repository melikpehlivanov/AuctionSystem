namespace Application.UnitTests.Setup
{
    using System;
    using AutoMapper;
    using Persistance;

    public class CommandTestBase : IDisposable
    {
        protected AuctionSystemDbContext Context { get; }

        protected IMapper Mapper { get; }

        public CommandTestBase()
        {
            this.Context = AuctionSystemContextFactory.Create();

            this.Mapper = TestSetup.InitializeMapper();
        }

        public void Dispose()
        {
            AuctionSystemContextFactory.Destroy(this.Context);
        }
    }
}