using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Queries.ListArticles;
using Application.Content.Domain.Entities;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.Content.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using AutoFixture;

namespace Application.Content.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class ListArticlesUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;

        public ListArticlesUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenNoArticles_WhenListArticles_ThenReturnsEmptyCollection()
        {
            
            //arrange
            _module.ClearModuleDatabaseTables();
            var listArticlesQuery = new ListArticlesQuery {};

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GivenANegativeOffset_WhenListArticles_ThenInvalidRequest()
        {
            
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                Offset = -1
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task GivenAnInvalidLimit_WhenListArticles_ThenInvalidRequest(int limit)
        {
            
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                Limit = limit
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
        }
        
        [Fact]
        public async Task GivenNonExistentUsername_WhenListArticlesByUsername_ThenNotFound()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                AuthorUsername = _module.AutoFixture.Create<string>()
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.NotFound);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNonExistentUsername_WhenListArticlesFavoritedByUsername_ThenNotFound()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                FavoritedByUsername = _module.AutoFixture.Create<string>()
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.NotFound);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNonExistentTag_WhenListArticlesTag_ThenNotFound()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                Tag = _module.AutoFixture.Create<string>()
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.NotFound);
            result.Response.Should().BeNull();
        }
        
        /*[Fact]
        public async Task GivenDefaultFilterCriteria_WhenListArticles_ThenReturnsMostRecentArticlesOrderedByDate()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery();

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.NotImplemented);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Count.Should().Be(2);
        }*/
        
        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenListArticlesByUsername_ThenReturnsArticlesByThatUser()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                AuthorUsername = _module.AuthenticatedUserUsername
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            var expectedArticles = _module.UserArticles[_module.AuthenticatedUserUsername];

            VerifyArticlesMatchExpectations(result, expectedArticles);
        }

        private void VerifyArticlesMatchExpectations(OperationResponse<ListArticlesQueryResult> result, List<ArticleEntity> expectedArticles)
        {
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Count.Should().Be(expectedArticles.Count);

            foreach (var expectedArticle in expectedArticles)
            {
                result.Response.Articles.Should().Contain(returnedArticle =>
                    returnedArticle.Author.Username == expectedArticle.Author.Username &&
                    returnedArticle.Title == expectedArticle.Title &&
                    returnedArticle.Description == expectedArticle.Description &&
                    returnedArticle.Body == expectedArticle.Body &&
                    returnedArticle.Favorited == expectedArticle.Favorited &&
                    returnedArticle.TagList.OrderBy(t => t).SequenceEqual(expectedArticle.TagList.Select(t => t.Tag).OrderBy(t => t))
                );
            }
        }

        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenListArticlesFavoritedByUsername_ThenReturnsFavoritedByThatUser()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                FavoritedByUsername = _module.AuthenticatedUserUsername
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            var expectedArticles = _module.FavoritedArticles;

            VerifyArticlesMatchExpectations(result, expectedArticles);
        }
        
        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenListArticlesByTag_ThenReturnsArticlesWithThatTag()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                Tag = _module.ExistingArticleTag1
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            var expectedArticles = _module.TaggedArticles[listArticlesQuery.Tag];

            VerifyArticlesMatchExpectations(result, expectedArticles);
        }
    }
}