using System;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.DTOs;
using Application.Content.Domain.Contracts.Operations.Commands.PublishArticle;
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
        private readonly PublishArticleCommand _publishArticleCommand;

        public PublishArticleTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _module = module;
            _publishArticleCommand = new PublishArticleCommand { NewArticle = new PublishArticleDTO
            {
                Body = _module.AutoFixture.Create<string>(),
                Title = $"{_module.AutoFixture.Create<string>()} {_module.AutoFixture.Create<string>()}",
                Description = _module.AutoFixture.Create<string>()
            }};
        }

        [Fact]
        public async Task GivenAnArticleToPublish_WhenPublishArticle_ThenReturnsArticle()
        {
            //arrange
            var testStartTime = DateTime.UtcNow;

            //act
            var result = await _module.Mediator.Send(_publishArticleCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Article.Should().NotBeNull();
            result.Response.Article.Slug.Should().Be(_publishArticleCommand.NewArticle.GetSlug());
            result.Response.Article.Title.Should().Be(_publishArticleCommand.NewArticle.Title);
            result.Response.Article.Description.Should().Be(_publishArticleCommand.NewArticle.Description);
            result.Response.Article.Body.Should().Be(_publishArticleCommand.NewArticle.Body);
            result.Response.Article.CreatedAt.Should().BeAfter(testStartTime);
            result.Response.Article.UpdatedAt.Should().BeAfter(testStartTime);
        }
        
        [Fact]
        public async Task GivenAnArticleWithoutTagsToPublish_WhenPublishArticle_ThenArticleContainsNoTags()
        {
            //act
            var result = await _module.Mediator.Send(_publishArticleCommand);

            //assert
            result.Response.Article.TagList.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GivenAnArticleWithTagsToPublish_WhenPublishArticle_ThenArticleContainsTags()
        {
            //arrange
            var tagList = new [] { _module.ExistingArticleTag1, _module.ExistingArticleTag2 };
            _publishArticleCommand.NewArticle.TagList = tagList;
            
            //act
            var result = await _module.Mediator.Send(_publishArticleCommand);

            //assert
            result.Response.Article.TagList.Should().BeEquivalentTo(tagList);
        }
        
        [Fact]
        public async Task GivenAnArticleToPublish_WhenPublishArticle_ThenArticleContainsAuthorProfile()
        {
            //act
            var result = await _module.Mediator.Send(_publishArticleCommand);

            //assert
            result.Response.Article.Author.Username.Should().Be(_module.AuthenticatedUserUsername);
            result.Response.Article.Author.Bio.Should().Be(_module.AuthenticatedUserBio);
            result.Response.Article.Author.Image.Should().Be(_module.AuthenticatedUserImage);
            result.Response.Article.Author.Following.Should().Be(true);
        }

        [Fact]
        public async Task GivenAnArticleToPublish_WhenPublishArticle_ThenArticleIsNotFavorited()
        {
            //act
            var result = await _module.Mediator.Send(_publishArticleCommand);

            //assert
            result.Response.Article.Favorited.Should().BeFalse();
            result.Response.Article.FavoritesCount.Should().Be(0);
        }
        
        [Fact]
        public async Task GivenAnArticleToPublishWithNoTitle_WhenPublishArticle_ThenInvalidRequest()
        {
            //arrange
            _publishArticleCommand.NewArticle.Title = null;

            //act
            var result = await _module.Mediator.Send(_publishArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenAnArticleToPublishWithNoDescription_WhenPublishArticle_ThenInvalidRequest()
        {
            //arrange
            _publishArticleCommand.NewArticle.Description = null;

            //act
            var result = await _module.Mediator.Send(_publishArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenAnArticleToPublishWithNoBody_WhenPublishArticle_ThenInvalidRequest()
        {
            //arrange
            _publishArticleCommand.NewArticle.Body = null;

            //act
            var result = await _module.Mediator.Send(_publishArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
    }
}