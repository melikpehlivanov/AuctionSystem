namespace Application.UnitTests.Categories.Queries
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Categories.Queries.List;
    using AutoMapper;
    using Common.Interfaces;
    using FluentAssertions;
    using Setup;
    using Xunit;

    [Collection("QueryCollection")]
    public class ListCategoriesQueryHandlerTests
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;

        public ListCategoriesQueryHandlerTests(QueryTestFixture fixture)
        {
            this.context = fixture.Context;
            this.mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetCategories_Should_Return_Correct_Count()
        {
            var handler = new ListCategoriesQueryHandler(this.context, this.mapper);
            var result = await handler.Handle(new ListCategoriesQuery(), CancellationToken.None);

            result
                .Data
                .Should()
                .HaveCount(this.context.Categories.Count());
        }
    }
}
