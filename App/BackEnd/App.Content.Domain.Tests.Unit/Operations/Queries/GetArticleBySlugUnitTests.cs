using System;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Queries.GetArticleBySlug;
using App.Content.Domain.Tests.Unit.Setup;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Content.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class GetArticleBySlugUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;

        public GetArticleBySlugUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenNoArticle_WhenGetArticle_ThenNotFound()
        {
            //arrange
            var getSingleArticleQuery = new GetArticleBySlugQuery { Slug = "some-non-existent-article" };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Result.Should().Be(OperationResult.NotFound);
        }

        [Fact]
        public async Task GivenAnArticle_WhenGetArticleBySlug_ThenReturnsArticle()
        {
            //arrange
            var testStartTime = DateTime.UtcNow;
            var getSingleArticleQuery = new GetArticleBySlugQuery { Slug = _module.NonFavoritedArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Article.Should().NotBeNull();
            result.Response.Article.Slug.Should().Be(_module.NonFavoritedArticleEntity.GetSlug());
            result.Response.Article.Title.Should().Be(_module.NonFavoritedArticleEntity.Title);
            result.Response.Article.Description.Should().Be(_module.NonFavoritedArticleEntity.Description);
            result.Response.Article.Body.Should().Be(_module.NonFavoritedArticleEntity.Body);
            result.Response.Article.CreatedAt.Should().BeBefore(testStartTime);
            result.Response.Article.UpdatedAt.Should().BeBefore(testStartTime);
        }

        [Fact]
        public async Task GivenAnArticle_WhenGetArticleBySlug_ThenArticleContainsTags()
        {
            //arrange
            var getSingleArticleQuery = new GetArticleBySlugQuery { Slug = _module.NonFavoritedArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.TagList.Should().BeEquivalentTo(new[] { _module.ExistingArticleTag2 });
        }

        [Fact]
        public async Task GivenAnArticle_WhenGetArticleBySlug_ThenArticleContainsAuthorProfile()
        {
            //arrange
            var getSingleArticleQuery = new GetArticleBySlugQuery { Slug = _module.NonFavoritedArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.Author.Username.Should().Be(_module.AuthenticatedUserUsername);
            result.Response.Article.Author.Bio.Should().Be(_module.AuthenticatedUserBio);
            result.Response.Article.Author.Image.Should().Be(_module.AuthenticatedUserImage);
            result.Response.Article.Author.Following.Should().Be(true);
        }

        [Fact]
        public async Task GivenAFavoritedArticle_WhenGetArticleBySlug_ThenArticleIsFavorited()
        {
            //arrange
            var getSingleArticleQuery = new GetArticleBySlugQuery { Slug = _module.FavoritedArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.Favorited.Should().BeTrue();
            result.Response.Article.FavoritesCount.Should().Be(1);
        }

        [Fact]
        public async Task GivenANonFavoritedArticle_WhenGetArticleBySlug_ThenArticleIsNotFavorited()
        {
            //arrange
            var getSingleArticleQuery = new GetArticleBySlugQuery { Slug = _module.NonFavoritedArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.Favorited.Should().BeFalse();
            result.Response.Article.FavoritesCount.Should().Be(0);
        }

        [Fact]
        public async Task GivenNoSlug_WhenGetArticle_ThenInvalidRequest()
        {
            //arrange
            var getSingleArticleQuery = new GetArticleBySlugQuery { Slug = null };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
    }
}