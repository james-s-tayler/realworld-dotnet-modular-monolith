using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using App.Feed.Domain.Contracts.Operations.Queries.GetFeed;
using App.Feed.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Feed.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(FeedModuleTestCollection))]
    public class GetFeedUnitTests : UnitTestBase
    {
        private readonly FeedModuleSetupFixture _module;
        
        public GetFeedUnitTests(FeedModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public async Task GivenOutOfRangeLimit_WhenGetFeed_ThenInvalidRequest(int limit)
        {
            //arrange
            var getFeedQuery = new GetFeedQuery { Limit = limit };
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
        }
        
        [Fact]
        public async Task GivenOutOfRangeOffset_WhenGetFeed_ThenInvalidRequest()
        {
            //arrange
            var getFeedQuery = new GetFeedQuery { Offset = -1 };
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
        }
        
        [Fact]
        public async Task GivenNotFollowingAnyUsers_WhenGetFeed_ThenEmptyFeed()
        {
            //arrange
            var getFeedQuery = new GetFeedQuery();
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
        }
    }
}