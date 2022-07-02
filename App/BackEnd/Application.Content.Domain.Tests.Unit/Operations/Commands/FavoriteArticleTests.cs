using System;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.DTOs;
using Application.Content.Domain.Contracts.Operations.Commands.FavoriteArticle;
using Application.Content.Domain.Tests.Unit.Setup;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Content.Domain.Tests.Unit.Operations.Commands
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class FavoriteArticleTests : TestBase
    {
        private readonly ContentModuleSetupFixture _module;
        private readonly FavoriteArticleCommand _favoriteArticleCommand;

        public FavoriteArticleTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _module = module;
            _favoriteArticleCommand = new FavoriteArticleCommand { Slug = _module.ExistingNonFavoritedArticleEntity.GetSlug() };
        }

        [Fact]
        public async Task GivenAnUnfavoritedArticle_WhenFavoriteArticle_ThenArticleFavorited()
        {
            //pre-assert
            _module.ExistingNonFavoritedArticleEntity.Favorited.Should().BeFalse();
            
            //act
            var result = await _module.Mediator.Send(_favoriteArticleCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Article.Should().NotBeNull();
            result.Response.Article.Slug.Should().Be(_favoriteArticleCommand.Slug);
            result.Response.Article.Favorited.Should().BeTrue();
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