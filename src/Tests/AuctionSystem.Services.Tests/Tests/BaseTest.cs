namespace AuctionSystem.Services.Tests.Tests
{
    using System;
    using AutoMapper;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Setup;

    public abstract class BaseTest
    {
        protected readonly IMapper mapper;

        protected BaseTest()
        {
            this.mapper = TestSetup.InitializeMapper();
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
