namespace Application.UnitTests.Setup
{
    using System;
    using AutoMapper;
    using global::Common.AutoMapping.Profiles;
    using Persistance;
    using Xunit;

    public class QueryTestFixture : IDisposable
    {
        public AuctionSystemDbContext Context { get; }
        public IMapper Mapper { get; }

        public QueryTestFixture()
        {
            this.Context = AuctionSystemContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DefaultProfile>();
            });

            this.Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            AuctionSystemContextFactory.Destroy(this.Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}