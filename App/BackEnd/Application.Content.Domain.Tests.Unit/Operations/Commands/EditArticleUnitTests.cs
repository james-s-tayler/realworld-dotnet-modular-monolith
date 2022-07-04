using System;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.EditArticle;
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
    public class EditArticleUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;
        private readonly EditArticleCommand _editArticleCommand;

        public EditArticleUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _editArticleCommand = _module.AutoFixture.Create<EditArticleCommand>();
            _editArticleCommand.Slug = _module.ExistingFavoritedArticleEntity.GetSlug();
        }

        [Fact]
        public async Task GivenAnArticleUpdate_WhenEditArticle_ThenArticleUpdated()
        {
            //arrange
            var testStartTime = DateTime.UtcNow;

            //act
            var result = await _module.Mediator.Send(_editArticleCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Article.Should().NotBeNull();
            result.Response.Article.Title.Should().Be(_editArticleCommand.UpdatedArticle.Title);
            result.Response.Article.Description.Should().Be(_editArticleCommand.UpdatedArticle.Description);
            result.Response.Article.Body.Should().Be(_editArticleCommand.UpdatedArticle.Body);
            result.Response.Article.CreatedAt.Should().BeBefore(testStartTime);
            result.Response.Article.UpdatedAt.Should().BeAfter(testStartTime);
        }

        [Fact]
        public async Task GivenAnArticle_WhenEditArticle_ThenArticleContainsAuthorProfile()
        {
            //act
            var result = await _module.Mediator.Send(_editArticleCommand);

            //assert
            result.Response.Article.Author.Username.Should().Be(_module.AuthenticatedUserUsername);
            result.Response.Article.Author.Bio.Should().Be(_module.AuthenticatedUserBio);
            result.Response.Article.Author.Image.Should().Be(_module.AuthenticatedUserImage);
            result.Response.Article.Author.Following.Should().Be(true);
        }

        [Fact]
        public async Task GivenNoSlug_WhenEditArticle_ThenInvalidRequest()
        {
            //arrange
            _editArticleCommand.Slug = null;

            //act
            var result = await _module.Mediator.Send(_editArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenAnArticleUpdateWithNullContent_WhenEditArticle_ThenInvalidRequest()
        {
            //arrange
            _editArticleCommand.UpdatedArticle.Title = null;
            _editArticleCommand.UpdatedArticle.Description = null;
            _editArticleCommand.UpdatedArticle.Body = null;

            //act
            var result = await _module.Mediator.Send(_editArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenAnArticleUpdateWithBodyOnly_WhenEditArticle_ThenArticleBodyUpdated()
        {
            //arrange
            _editArticleCommand.UpdatedArticle.Title = null;
            _editArticleCommand.UpdatedArticle.Description = null;
            _editArticleCommand.UpdatedArticle.Body = _module.AutoFixture.Create<string>();

            //act
            var result = await _module.Mediator.Send(_editArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Article.Should().NotBeNull();
            result.Response.Article.Title.Should().Be(_module.ExistingFavoritedArticleEntity.Title);
            result.Response.Article.Description.Should().Be(_module.ExistingFavoritedArticleEntity.Description);
            result.Response.Article.Body.Should().Be(_editArticleCommand.UpdatedArticle.Body);
        }
        
        [Fact]
        public async Task GivenAnArticleUpdateWithEmptyContent_WhenEditArticle_ThenArticleUpdated()
        {
            //arrange
            _editArticleCommand.UpdatedArticle.Title = "";
            _editArticleCommand.UpdatedArticle.Description = "";
            _editArticleCommand.UpdatedArticle.Body = "";

            //act
            var result = await _module.Mediator.Send(_editArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
        }
    }
}