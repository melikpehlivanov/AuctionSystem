namespace Application.UnitTests.Setup
{
    using System;
    using AutoMapper;
    using Persistance;
    using Xunit;

    public class QueryTestFixture : IDisposable
    {
        public AuctionSystemDbContext Context { get; }
        public IMapper Mapper { get; }

        public QueryTestFixture()
        {
            this.Context = AuctionSystemContextFactory.Create();

            this.Mapper = TestSetup.InitializeMapper();
        }

        public void Dispose()
        {
            AuctionSystemContextFactory.Destroy(this.Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}