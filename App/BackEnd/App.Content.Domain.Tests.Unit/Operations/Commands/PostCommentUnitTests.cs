using System;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.PostComment;
using App.Content.Domain.Tests.Unit.Setup;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Content.Domain.Tests.Unit.Operations.Commands
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class PostCommentUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;
        private readonly PostCommentCommand _postCommentCommand;

        public PostCommentUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _postCommentCommand = _module.AutoFixture.Create<PostCommentCommand>();
            _postCommentCommand.ArticleSlug = _module.FavoritedArticleEntity.GetSlug();
        }

        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenPostComment_ThenReturnsComment()
        {
            //arrange
            var testStartTime = DateTime.UtcNow;

            //act
            var result = await _module.Mediator.Send(_postCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Comment.Should().NotBeNull();
            result.Response.Comment.Body.Should().Be(_postCommentCommand.NewComment.Body);
            result.Response.Comment.CreatedAt.Should().BeAfter(testStartTime);
            result.Response.Comment.UpdatedAt.Should().BeAfter(testStartTime);
        }
        
        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenPostComment_ThenReturnsCommenterProfile()
        {
            //act
            var result = await _module.Mediator.Send(_postCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Comment.Should().NotBeNull();
            result.Response.Comment.Author.Should().NotBeNull();
            result.Response.Comment.Author.Username.Should().Be(_module.AuthenticatedUserUsername);
            result.Response.Comment.Author.Bio.Should().Be(_module.AuthenticatedUserBio);
            result.Response.Comment.Author.Image.Should().Be(_module.AuthenticatedUserImage);
            result.Response.Comment.Author.Following.Should().BeTrue();
        }
        
        [Fact]
        public async Task GivenAnUnauthenticatedUser_WhenPostComment_ThenNotAuthenticated()
        {
            //arrange
            _module.WithUnauthenticatedUserContext();
            
            //act
            var result = await _module.Mediator.Send(_postCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.NotAuthenticated);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNoSlug_WhenPostComment_ThenInvalidRequest()
        {
            //arrange
            _postCommentCommand.ArticleSlug = null;
            
            //act
            var result = await _module.Mediator.Send(_postCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNoComment_WhenPostComment_ThenInvalidRequest()
        {
            //arrange
            _postCommentCommand.NewComment = null;
            
            //act
            var result = await _module.Mediator.Send(_postCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNoCommentBody_WhenPostComment_ThenInvalidRequest()
        {
            //arrange
            _postCommentCommand.NewComment.Body = null;
            
            //act
            var result = await _module.Mediator.Send(_postCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenArticleDoesntExist_WhenPostComment_ThenValidationError()
        {
            //arrange
            _postCommentCommand.ArticleSlug = _module.AutoFixture.Create<string>();
            
            //act
            var result = await _module.Mediator.Send(_postCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
    }
}