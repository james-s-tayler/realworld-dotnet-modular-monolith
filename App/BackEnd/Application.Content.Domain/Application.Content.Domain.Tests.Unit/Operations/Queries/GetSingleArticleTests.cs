using System;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.Content.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Content.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class GetSingleArticleTests : TestBase
    {
        private readonly ContentModuleSetupFixture _module;

        public GetSingleArticleTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenNoArticle_WhenGetArticle_ThenNotFound()
        {
            
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = "some-non-existent-article" };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
        }
        
        [Fact]
        public async Task GivenAnArticle_WhenGetArticleBySlug_ThenReturnsArticle()
        {
            //arrange
            var testStartTime = DateTime.UtcNow;
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Article.Should().NotBeNull();
            result.Response.Article.Slug.Should().Be(_module.ExistingArticleEntity.GetSlug());
            result.Response.Article.Title.Should().Be(_module.ExistingArticleEntity.Title);
            result.Response.Article.Description.Should().Be(_module.ExistingArticleEntity.Description);
            result.Response.Article.Body.Should().Be(_module.ExistingArticleEntity.Body);
            result.Response.Article.CreatedAt.Should().BeAfter(testStartTime);
            result.Response.Article.UpdatedAt.Should().BeAfter(testStartTime);
        }
        
        [Fact]
        public async Task GivenAnArticle_WhenGetArticleBySlug_ThenArticleContainsTags()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.TagList.Should().BeEquivalentTo(new []{_module.ExistingArticleTag1, _module.ExistingArticleTag2});
        }
        
        [Fact]
        public async Task GivenAnArticle_WhenGetArticleBySlug_ThenArticleContainsAuthorProfile()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.Author.Should().NotBeNull();
        }
        
        [Fact]
        public async Task GivenAFavoritedArticle_WhenGetArticleBySlug_ThenArticleIsFavorited()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.Favorited.Should().BeTrue();
        }
        
        [Fact]
        public async Task GivenANonFavoritedArticle_WhenGetArticleBySlug_ThenArticleIsNotFavorited()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.Favorited.Should().BeFalse();
        }
        
        [Fact]
        public async Task GivenANonFavoritedArticle_WhenGetArticleBySlug_ThenFavoritesCountIsZero()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.FavoritesCount.Should().Be(0);
        }
        
        [Fact]
        public async Task GivenAFavoritedArticle_WhenGetArticleBySlug_ThenFavoritesCountIsNonZero()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.FavoritesCount.Should().Be(1);
        }
        
        [Fact]
        public async Task GivenNoSlug_WhenGetArticle_ThenValidationError()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = null };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
    }
}