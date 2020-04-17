namespace Application.UnitTests.Setup
{
    using System;
    using Persistance;

    public class CommandTestBase : IDisposable
    {
        protected readonly AuctionSystemDbContext Context;

        public CommandTestBase()
        {
            this.Context = AuctionSystemContextFactory.Create();
        }

        public void Dispose()
        {
            AuctionSystemContextFactory.Destroy(this.Context);
        }
    }
}