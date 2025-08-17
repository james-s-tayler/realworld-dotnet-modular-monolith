using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.FavoriteArticle;
using App.Content.Domain.Tests.Unit.Setup;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Content.Domain.Tests.Unit.Operations.Commands
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class FavoriteArticleUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;
        private readonly FavoriteArticleCommand _favoriteArticleCommand;

        public FavoriteArticleUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _favoriteArticleCommand = new FavoriteArticleCommand { Slug = _module.NonFavoritedArticleEntity.GetSlug() };
        }

        [Fact]
        public async Task GivenAnUnfavoritedArticle_WhenFavoriteArticle_ThenArticleFavorited()
        {
            //pre-assert
            _module.NonFavoritedArticleEntity.Favorited.Should().BeFalse();
            _module.NonFavoritedArticleEntity.FavoritesCount.Should().Be(0);

            //act
            var result = await _module.Mediator.Send(_favoriteArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Article.Should().NotBeNull();
            result.Response.Article.Slug.Should().Be(_favoriteArticleCommand.Slug);
            result.Response.Article.Favorited.Should().BeTrue();
            result.Response.Article.FavoritesCount.Should().Be(1);
        }

        [Fact]
        public async Task GivenSlugIsMissing_WhenFavoriteArticle_ThenInvalidRequest()
        {
            //arrange
            _favoriteArticleCommand.Slug = null;

            //act
            var result = await _module.Mediator.Send(_favoriteArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
    }
}