using System;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.DTOs;
using Application.Content.Domain.Contracts.Operations.Commands.PublishArticle;
using Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using Application.Content.Domain.Tests.Unit.Setup;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using AutoFixture;

namespace Application.Content.Domain.Tests.Unit.Operations.Commands
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class PublishArticleTests : TestBase
    {
        private readonly ContentModuleSetupFixture _module;

        public PublishArticleTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenAnArticleToPublish_WhenPublishArticle_ThenReturnsArticle()
        {
            //arrange
            var testStartTime = DateTime.UtcNow;
            var publishArticleCommand = new PublishArticleCommand { NewArticle = new PublishArticleDTO
            {
                Body = _module.AutoFixture.Create<string>(),
                Title = $"{_module.AutoFixture.Create<string>()} {_module.AutoFixture.Create<string>()}",
                Description = _module.AutoFixture.Create<string>()
            }};
            
            //act
            var result = await _module.Mediator.Send(publishArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Article.Should().NotBeNull();
            result.Response.Article.Slug.Should().Be(publishArticleCommand.NewArticle.GetSlug());
            result.Response.Article.Title.Should().Be(publishArticleCommand.NewArticle.Title);
            result.Response.Article.Description.Should().Be(publishArticleCommand.NewArticle.Description);
            result.Response.Article.Body.Should().Be(publishArticleCommand.NewArticle.Body);
            result.Response.Article.CreatedAt.Should().BeAfter(testStartTime);
            result.Response.Article.UpdatedAt.Should().BeAfter(testStartTime);
        }
        
        /*[Fact]
        public async Task GivenAnArticle_WhenGetArticleBySlug_ThenArticleContainsTags()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingNonFavoritedArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.TagList.Should().BeEquivalentTo(new []{_module.ExistingArticleTag1, _module.ExistingArticleTag2});
        }*/
        
        /*[Fact]
        public async Task GivenAnArticle_WhenGetArticleBySlug_ThenArticleContainsAuthorProfile()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingNonFavoritedArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.Author.Username.Should().Be(_module.AuthenticatedUserUsername);
            result.Response.Article.Author.Bio.Should().Be(_module.AuthenticatedUserBio);
            result.Response.Article.Author.Image.Should().Be(_module.AuthenticatedUserImage);
            result.Response.Article.Author.Following.Should().Be(true);
        
        }*/

        /*[Fact]
        public async Task GivenANonFavoritedArticle_WhenGetArticleBySlug_ThenArticleIsNotFavorited()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = _module.ExistingNonFavoritedArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Response.Article.Favorited.Should().BeFalse();
            result.Response.Article.FavoritesCount.Should().Be(0);
        }*/
        
        /*[Fact]
        public async Task GivenNoSlug_WhenGetArticle_ThenInvalidRequest()
        {
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = null };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }*/
    }
}