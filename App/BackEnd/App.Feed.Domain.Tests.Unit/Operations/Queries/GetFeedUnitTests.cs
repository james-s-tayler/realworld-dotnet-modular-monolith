using System.Linq;
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
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public async Task GivenLimit_WhenGetFeed_ThenLimitNumberOfArticlesReturned(int limit)
        {
            //arrange
            var getFeedQuery = new GetFeedQuery { Limit = limit };
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.FeedArticles.Count.Should().Be(limit <= _module.FollowedUserArticles.Count ? limit : _module.FollowedUserArticles.Count);
        }
        
        [Fact]
        public async Task GivenNegativeOffset_WhenGetFeed_ThenInvalidRequest()
        {
            //arrange
            var getFeedQuery = new GetFeedQuery { Offset = -1 };
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
        }
        
        [Fact]
        public async Task GivenUnauthenticatedUser_WhenGetFeed_ThenNotAuthenticated()
        {
            //arrange
            _module.WithUnauthenticatedUserContext();
            var getFeedQuery = new GetFeedQuery();
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.NotAuthenticated);
        }
        
        [Fact]
        public async Task GivenOffsetOutOfBounds_WhenGetFeed_ThenNoArticles()
        {
            //arrange
            var getFeedQuery = new GetFeedQuery { Offset = _module.FollowedUserArticles.Count + 1 };
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.FeedArticles.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GivenLimitAndOffset_WhenGetFeed_ThenReturnsLimitNumberOfArticles()
        {
            //arrange
            var getFeedQuery = new GetFeedQuery
            {
                Limit = 1,
                Offset = 0
            };
            
            //act
            var previousResult = await _module.Mediator.Send(getFeedQuery);

            for (var i = 1; i < _module.FollowedUserArticles.Count; i++)
            {
                getFeedQuery.Offset = i;
                
                //act
                var result = await _module.Mediator.Send(getFeedQuery);

                //assert
                result.Result.Should().Be(OperationResult.Success);
                result.Response.Should().NotBeNull();
                result.Response.FeedArticles.Count.Should().Be(getFeedQuery.Limit);
                var currentArticle = result.Response.FeedArticles.Single();
                var previousArticle = previousResult.Response.FeedArticles.Single();
                    
                currentArticle.Title.Should().NotBe(previousArticle.Title);
                currentArticle.CreatedAt.Should().BeBefore(previousArticle.CreatedAt);
                
                previousResult = result;
            }
        }
        
        
        [Fact]
        public async Task GivenFollowingUsers_WhenGetFeed_ThenContainsArticlesFromFollowedUsers()
        {
            //arrange
            var getFeedQuery = new GetFeedQuery();
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.FeedArticles.Count.Should().Be(_module.FollowedUserArticles.Count);
        }
        
        [Fact]
        public async Task GivenFollowingUsers_WhenGetFeed_ThenArticlesAreOnlyFromFollowedUsers()
        {
            //arrange
            var getFeedQuery = new GetFeedQuery();
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            foreach (var article in result.Response.FeedArticles)
            {
                article.Author.Following.Should().BeTrue();
            }
        }
        
        [Fact]
        public async Task GivenFeed_WhenGetFeed_ThenArticlesInChronologicalOrder()
        {
            //arrange
            var getFeedQuery = new GetFeedQuery();
            
            //act
            var result = await _module.Mediator.Send(getFeedQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();

            var articles = result.Response.FeedArticles;
            var previousArticle = articles[0];
            for (var i = 1; i < articles.Count; i++)
            {
                var currentArticle = articles[i];
                currentArticle.CreatedAt.Should().BeBefore(previousArticle.CreatedAt);
                previousArticle = currentArticle;
            }
        }
    }
}