namespace AuctionSystem.Services.Tests.Tests
{
    using System;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Setup;

    public abstract class BaseTest
    {
        protected BaseTest()
        {
            TestSetup.InitializeMapper();
        }

        protected AuctionSystemDbContext DatabaseInstance
        {
            get
            {
                var options = new DbContextOptionsBuilder<AuctionSystemDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .EnableSensitiveDataLogging()
                    .Options;

                return new AuctionSystemDbContext(options);
            }
        }
    }
}
