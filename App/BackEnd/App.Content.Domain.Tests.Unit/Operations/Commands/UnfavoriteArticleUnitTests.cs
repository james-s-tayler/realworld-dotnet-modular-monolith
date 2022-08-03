using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.UnfavoriteArticle;
using App.Content.Domain.Tests.Unit.Setup;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Content.Domain.Tests.Unit.Operations.Commands
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class UnfavoriteArticleUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;
        private readonly UnfavoriteArticleCommand _unfavoriteArticleCommand;

        public UnfavoriteArticleUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _unfavoriteArticleCommand = new UnfavoriteArticleCommand { Slug = _module.FavoritedArticleEntity.GetSlug() };
        }

        [Fact]
        public async Task GivenAFavoritedArticle_WhenUnfavoriteArticle_ThenArticleUnfavorited()
        {
            //pre-assert
            _module.FavoritedArticleEntity.Favorited.Should().BeTrue();

            //act
            var result = await _module.Mediator.Send(_unfavoriteArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Article.Should().NotBeNull();
            result.Response.Article.Slug.Should().Be(_unfavoriteArticleCommand.Slug);
            result.Response.Article.Favorited.Should().BeFalse();
        }

        [Fact]
        public async Task GivenSlugIsMissing_WhenFavoriteArticle_ThenInvalidRequest()
        {
            //arrange
            _unfavoriteArticleCommand.Slug = null;

            //act
            var result = await _module.Mediator.Send(_unfavoriteArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
    }
}