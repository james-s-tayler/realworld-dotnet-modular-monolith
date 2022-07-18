using System;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.DeleteComment;
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
    public class DeleteCommentUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;
        private readonly DeleteCommentCommand _deleteCommentCommand;

        public DeleteCommentUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _deleteCommentCommand = new DeleteCommentCommand
            {
                ArticleSlug = _module.CommentedOnArticleEntity.GetSlug(),
                CommentId = _module.CommentEntity.Id
            };
        }

        [Fact]
        public async Task GivenCommentBelongsToAuthenticatedUser_WhenAuthenticatedUserDeleteComment_ThenDeleted()
        {
            //act
            var result = await _module.Mediator.Send(_deleteCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
        }
        
        [Fact]
        public async Task GivenAnUnauthenticatedUser_WhenDeleteComment_ThenNotAuthenticated()
        {
            //arrange
            _module.WithUnauthenticatedUserContext();
            
            //act
            var result = await _module.Mediator.Send(_deleteCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.NotAuthenticated);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNoSlug_WhenDeleteComment_ThenInvalidRequest()
        {
            //arrange
            _deleteCommentCommand.ArticleSlug = null;
            
            //act
            var result = await _module.Mediator.Send(_deleteCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNoSuchCommentId_WhenDeleteComment_ThenValidationError()
        {
            //arrange
            _deleteCommentCommand.CommentId = Int32.MaxValue;
            
            //act
            var result = await _module.Mediator.Send(_deleteCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }

        [Fact]
        public async Task GivenArticleDoesntExist_WhenDeleteComment_ThenValidationError()
        {
            //arrange
            _deleteCommentCommand.ArticleSlug = _module.AutoFixture.Create<string>();
            
            //act
            var result = await _module.Mediator.Send(_deleteCommentCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
    }
}