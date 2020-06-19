namespace Application.UnitTests.Items.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Items.Queries.List;
    using AuctionSystem.Infrastructure;
    using AutoMapper;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using FluentAssertions;
    using global::Common;
    using Moq;
    using Setup;
    using Xunit;

    [Collection("QueryCollection")]
    public class ListItemsQueryHandlerTests
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly IDateTime dateTime;

        private readonly ListItemsQueryHandler handler;

        public ListItemsQueryHandlerTests(QueryTestFixture fixture)
        {
            this.context = fixture.Context;
            this.mapper = fixture.Mapper;

            var mockedDatetime = new Mock<IDateTime>();
            mockedDatetime.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
            mockedDatetime.Setup(x => x.Now).Returns(DateTime.Now);
            this.dateTime = mockedDatetime.Object;

            this.handler = new ListItemsQueryHandler(this.context, this.dateTime, this.mapper);
        }

        [Fact]
        public async Task ListItems_Given_QueryFilter_With_EndTime_Should_Return_CorrectEntities()
        {
            var expectedEndTime = this.dateTime.UtcNow.AddDays(5);
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow,
                EndTime = this.dateTime.UtcNow.AddDays(2),
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = DataConstants.SampleAdminUserId
            });
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow,
                EndTime = this.dateTime.UtcNow.AddDays(4),
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = DataConstants.SampleAdminUserId
            });
            await this.context.SaveChangesAsync(CancellationToken.None);

            var result = await this.handler.Handle(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    EndTime = expectedEndTime
                }
            }, CancellationToken.None);

            result
                .Data
                .All(x => x.As<ListItemsResponseModel>().EndTime <= expectedEndTime)
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ListItems_Given_QueryFilter_With_GetLiveItems_Should_Return_CorrectEntities()
        {
            const int liveItemsCount = 2;
            await this.SeedLiveItems(liveItemsCount);

            var result = await this.handler.Handle(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    GetLiveItems = true
                }
            }, CancellationToken.None);

            result
                .Data
                .Should()
                .HaveCount(liveItemsCount);
        }

        [Fact]
        public async Task ListItems_Given_QueryFilter_With_MinimumPicturesCount_Should_Return_CorrectEntities()
        {
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                EndTime = this.dateTime.UtcNow.AddDays(10),
                Pictures = new List<Picture> { new Picture(), new Picture(), new Picture() }
            });
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                EndTime = this.dateTime.UtcNow.AddDays(10),
                Pictures = new List<Picture> { new Picture(), new Picture(), new Picture() }
            });

            const int expectedMinNumberPicturesCount = 3;
            var result = await this.handler.Handle(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    MinimumPicturesCount = expectedMinNumberPicturesCount
                }
            }, CancellationToken.None);

            result
                .Data
                .All(x => x.As<ListItemsResponseModel>().Pictures.Count >= expectedMinNumberPicturesCount)
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ListItems_Given_QueryFilter_With_MinPrice_Should_Return_CorrectEntities()
        {
            const decimal expectedPrice = 500;
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = expectedPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = DataConstants.SampleAdminUserId
            });
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = expectedPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = DataConstants.SampleAdminUserId
            });
            await this.context.SaveChangesAsync(CancellationToken.None);

            var result = await this.handler.Handle(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    MinPrice = expectedPrice
                }
            }, CancellationToken.None);

            result
                .Data
                .All(x => x.As<ListItemsResponseModel>().StartingPrice >= expectedPrice)
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ListItems_Given_QueryFilter_With_MaxPrice_Should_Return_CorrectEntities()
        {
            const decimal expectedPrice = 1000;
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = expectedPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = DataConstants.SampleAdminUserId
            });
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = expectedPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = DataConstants.SampleAdminUserId
            });
            await this.context.SaveChangesAsync(CancellationToken.None);

            var result = await this.handler.Handle(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    MinPrice = expectedPrice
                }
            }, CancellationToken.None);

            result
                .Data
                .All(x => x.As<ListItemsResponseModel>().StartingPrice <= expectedPrice)
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ListItems_Given_QueryFilter_With_StartTime_Should_Return_CorrectEntities()
        {
            var expectedStartTime = this.dateTime.UtcNow.AddDays(5);
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = expectedStartTime.AddDays(1),
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = DataConstants.SampleAdminUserId
            });
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = expectedStartTime,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = DataConstants.SampleAdminUserId
            });
            await this.context.SaveChangesAsync(CancellationToken.None);

            var result = await this.handler.Handle(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    StartTime = expectedStartTime
                }
            }, CancellationToken.None);

            result
                .Data
                .All(x => x.As<ListItemsResponseModel>().StartTime >= expectedStartTime)
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ListItems_Given_QueryFilter_With_SubCategoryId_Should_Return_CorrectEntities()
        {
            var expectedSubCategoryId = Guid.NewGuid();
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                EndTime = this.dateTime.UtcNow.AddDays(10),
                SubCategoryId = expectedSubCategoryId
            });
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                EndTime = this.dateTime.UtcNow.AddDays(10),
                SubCategoryId = expectedSubCategoryId
            });

            var result = await this.handler.Handle(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    SubCategoryId = expectedSubCategoryId
                }
            }, CancellationToken.None);

            result
                .Data
                .All(x => x.As<ListItemsResponseModel>().SubCategoryId == expectedSubCategoryId)
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ListItems_Given_QueryFilter_With_Title_Should_Return_CorrectEntities()
        {
            const string expectedTitle = "Filtered title";
            await this.context.Items.AddAsync(new Item { Title = expectedTitle });
            await this.context.SaveChangesAsync(CancellationToken.None);

            var result = await this.handler.Handle(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    Title = expectedTitle
                }
            }, CancellationToken.None);

            result
                .Data
                .All(x => x.As<ListItemsResponseModel>().Title.Contains(expectedTitle))
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ListItems_Given_QueryFilter_With_UserId_Should_Return_CorrectEntities()
        {
            const string expectedUserId = "expectedUserId";
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = expectedUserId
            });
            await this.context.Items.AddAsync(new Item
            {
                Title = DataConstants.SampleItemTitle,
                Description = DataConstants.SampleItemDescription,
                StartingPrice = DataConstants.SampleItemStartingPrice,
                MinIncrease = DataConstants.SampleItemMinIncrease,
                StartTime = this.dateTime.UtcNow,
                EndTime = DataConstants.SampleItemEndTime,
                SubCategoryId = DataConstants.SampleSubCategoryId,
                UserId = expectedUserId
            });
            await this.context.SaveChangesAsync(CancellationToken.None);

            var result = await this.handler.Handle(new ListItemsQuery
            {
                Filters = new ListAllItemsQueryFilter
                {
                    UserId = expectedUserId
                }
            }, CancellationToken.None);

            result
                .Data
                .All(x => x.As<ListItemsResponseModel>().UserId == expectedUserId)
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task ListItems_Should_Return_Correct_Model()
        {
            var result = await this.handler.Handle(new ListItemsQuery(), CancellationToken.None);

            result
                .Should()
                .BeOfType<PagedResponse<ListItemsResponseModel>>();
        }

        private async Task SeedLiveItems(int count)
        {
            var items = new List<Item>();
            for (var i = 1; i <= count; i++)
            {
                var item = new Item
                {
                    Title = DataConstants.SampleItemTitle,
                    Description = DataConstants.SampleItemDescription,
                    StartingPrice = DataConstants.SampleItemStartingPrice,
                    MinIncrease = DataConstants.SampleItemMinIncrease,
                    StartTime = this.dateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                    EndTime = this.dateTime.UtcNow.AddDays(10),
                    UserId = DataConstants.SampleUserId
                };
                items.Add(item);
            }

            await this.context.Items.AddRangeAsync(items);
            await this.context.SaveChangesAsync(CancellationToken.None);
        }
    }
}