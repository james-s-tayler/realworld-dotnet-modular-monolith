using System;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Queries.GetArticleComments;
using App.Content.Domain.Tests.Unit.Setup;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.DTOs;
using App.Users.Domain.Contracts.Operations.Queries.GetProfile;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace App.Content.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class GetArticleCommentsUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;

        public GetArticleCommentsUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenNoArticle_WhenGetComments_ThenNoComment()
        {
            //arrange
            var getArticleCommentsQuery = new GetArticleCommentsQuery { Slug = "some-non-existent-article" };

            //act
            var result = await _module.Mediator.Send(getArticleCommentsQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Comments.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GivenNoSlug_WhenGetComments_ThenInvalidRequest()
        {
            //arrange
            var getArticleCommentsQuery = new GetArticleCommentsQuery { Slug = null };

            //act
            var result = await _module.Mediator.Send(getArticleCommentsQuery);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenAnArticleWithNoComments_WhenGetComments_ThenNoComments()
        {
            //arrange
            var getArticleCommentsQuery = new GetArticleCommentsQuery { Slug = _module.FavoritedArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getArticleCommentsQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Comments.Should().NotBeNull();
            result.Response.Comments.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GivenAnArticleWithComments_WhenGetComments_ThenReturnsComments()
        {
            //arrange
            var testStartTime = DateTime.UtcNow;
            var getArticleCommentsQuery = new GetArticleCommentsQuery { Slug = _module.CommentedOnArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getArticleCommentsQuery);

            //assert
            result.Response.Comments.Should().NotBeNull();
            result.Response.Comments.Should().NotBeEmpty();
            result.Response.Comments.Count.Should().Be(1);
            result.Response.Comments[0].Id.Should().Be(_module.CommentEntity.Id).And.BePositive();
            result.Response.Comments[0].Body.Should().Be(_module.CommentEntity.Body);
            result.Response.Comments[0].CreatedAt.Should().BeBefore(testStartTime);
            result.Response.Comments[0].UpdatedAt.Should().BeBefore(testStartTime);
        }
        
        [Fact]
        public async Task GivenAnArticleWithComments_WhenGetComments_ThenCommentsContainAuthorProfile()
        {
            //arrange
            var getArticleCommentsQuery = new GetArticleCommentsQuery { Slug = _module.CommentedOnArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getArticleCommentsQuery);

            //assert
            result.Response.Comments.Should().NotBeNull();
            result.Response.Comments.Should().NotBeEmpty();
            result.Response.Comments[0].Author.Username.Should().Be(_module.AuthenticatedUserUsername);
            result.Response.Comments[0].Author.Bio.Should().Be(_module.AuthenticatedUserBio);
            result.Response.Comments[0].Author.Image.Should().Be(_module.AuthenticatedUserImage);
            result.Response.Comments[0].Author.Following.Should().Be(true);
        }
        
        [Fact]
        public async Task GivenAnUnauthenticatedUser_WhenGetComments_ThenCommentsReturned()
        {
            //arrange
            _module.WithUnauthenticatedUserContext();
            var testStartTime = DateTime.UtcNow;
            var getArticleCommentsQuery = new GetArticleCommentsQuery { Slug = _module.CommentedOnArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getArticleCommentsQuery);

            //assert
            result.Response.Comments.Should().NotBeNull();
            result.Response.Comments.Should().NotBeEmpty();
            result.Response.Comments.Count.Should().Be(1);
            result.Response.Comments[0].Id.Should().Be(_module.CommentEntity.Id);
            result.Response.Comments[0].Body.Should().Be(_module.CommentEntity.Body);
            result.Response.Comments[0].CreatedAt.Should().BeBefore(testStartTime);
            result.Response.Comments[0].UpdatedAt.Should().BeBefore(testStartTime);
        }
        
        [Fact]
        public async Task GivenProfileNotFollowed_WhenGetComments_ThenNotFollowingCommenter()
        {
            //arrange 
            var existingUserProfile = new ProfileDTO
            {
                Username = _module.AuthenticatedUserUsername,
                Bio = _module.AuthenticatedUserBio,
                Image = _module.AuthenticatedUserImage,
                Following = false
            };

            var getProfileQueryResult = new GetProfileQueryResult { Profile = existingUserProfile };

            _module.SocialService.Setup(service => service.GetProfile(It.Is<string>(s => s.Equals(existingUserProfile.Username))))
                .ReturnsAsync(OperationResponseFactory.Success(getProfileQueryResult));
            
            var getArticleCommentsQuery = new GetArticleCommentsQuery { Slug = _module.CommentedOnArticleEntity.GetSlug() };

            //act
            var result = await _module.Mediator.Send(getArticleCommentsQuery);

            //assert
            result.Response.Comments[0].Author.Following.Should().BeFalse();
        }
    }
}