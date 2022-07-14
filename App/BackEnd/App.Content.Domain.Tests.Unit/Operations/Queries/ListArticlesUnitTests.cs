using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Queries.ListArticles;
using App.Content.Domain.Entities;
using App.Content.Domain.Tests.Unit.Setup;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using AutoFixture;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace App.Content.Domain.Tests.Unit.Operations.Queries
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
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Should().BeEmpty();
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
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Should().BeEmpty();
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
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GivenFilterByLimit_WhenListArticles_ThenReturnsLimitNumberOfArticles()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                Limit = 2
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Count.Should().Be(listArticlesQuery.Limit);
        }
        
        [Fact]
        public async Task GivenFilterByLimitAndOffset_WhenListArticles_ThenReturnsLimitNumberOfArticles()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                Limit = 1,
                Offset = 0
            };
            var previousResult = await _module.Mediator.Send(listArticlesQuery);
            
            for (var i = 1; i < (await _module.ArticleRepository.GetAll()).Count(); i++)
            {
                listArticlesQuery.Offset = i;
                
                //act
                var result = await _module.Mediator.Send(listArticlesQuery);

                //assert
                result.Result.Should().Be(OperationResult.Success);
                result.Response.Should().NotBeNull();
                result.Response.Articles.Count.Should().Be(listArticlesQuery.Limit);
                var currentArticle = result.Response.Articles.Single();
                var previousArticle = previousResult.Response.Articles.Single();
                    
                currentArticle.Title.Should().NotBe(previousArticle.Title);
                currentArticle.CreatedAt.Should().BeAfter(previousArticle.CreatedAt);
                
                previousResult = result;
            }
        }
        
        [Fact]
        public async Task GivenFilterByOffsetOutOfBounds_WhenListArticles_Then()
        {
            //arrange
            var outOfBoundsOffset = (await _module.ArticleRepository.GetAll()).Count() + 1;
            
            var listArticlesQuery = new ListArticlesQuery
            {
                Offset = outOfBoundsOffset
            };
            
            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GivenDefaultFilter_WhenListArticles_ThenReturnsMostRecentArticlesOrderedByDate()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery();

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Should().NotBeEmpty();

            var previousArticle = result.Response.Articles[0];
            
            for (var i = 1; i < result.Response.Articles.Count; i++)
            {
                var currentArticle = result.Response.Articles[i];
                currentArticle.CreatedAt.Should().BeAfter(previousArticle.CreatedAt);

                previousArticle = currentArticle;
            }
        }
        
        [Fact]
        public async Task GivenAnUnauthenticatedUser_WhenListArticles_ThenNotFollowingAnyAuthorsAndNoArticlesFavorited()
        {
            //arrange
            _module.ClearModuleDatabaseTables();
            _module.UserArticles.Clear();
            _module.FavoritedArticles.Clear();
            _module.TaggedArticles.Clear();
            _module.SocialService.Reset();
            _module.WithUnauthenticatedUserContext();
            await _module.WithUserAndArticles();
            await _module.WithUserAndArticles();
            await _module.WithUserAndArticles();
            
            var listArticlesQuery = new ListArticlesQuery();

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Count.Should().Be((await _module.ArticleRepository.GetAll()).Count());
            result.Response.Articles.Should().AllSatisfy(article => article.Favorited.Should().BeFalse());
            result.Response.Articles.Should().AllSatisfy(article => article.Author.Following.Should().BeFalse());
        }
        
        [Fact]
        public async Task GivenFilterByAuthor_WhenListArticlesByUsername_ThenReturnsMatchingArticles()
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
        public async Task GivenFilterByFavoritedBy_WhenListArticles_ThenReturnsMatchingArticles()
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
        public async Task GivenFilterByTag_WhenListArticles_ThenReturnsMatchingArticles()
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
        
        [Fact]
        public async Task GivenFilterByTagAuthorAndFavoritedBy_WhenListArticles_ThenReturnsMatchingArticles()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                FavoritedByUsername = _module.AuthenticatedUserUsername,
                AuthorUsername = _module.UserArticles.Keys.First(username => username != _module.AuthenticatedUserUsername),
                Tag = _module.ExistingArticleTag1
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            var expectedArticles = _module.UserArticles[listArticlesQuery.AuthorUsername].Where(article =>
                    article.Favorited &&
                    article.TagList.Any(tag => tag.Tag == listArticlesQuery.Tag)
                ).ToList();

            VerifyArticlesMatchExpectations(result, expectedArticles);
        }
        
        [Fact]
        public async Task GivenFilterByTagAndAuthor_WhenListArticles_ThenReturnsMatchingArticles()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                AuthorUsername = _module.UserArticles.Keys.First(username => username != _module.AuthenticatedUserUsername),
                Tag = _module.ExistingArticleTag1
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            
            var expectedArticles = _module.UserArticles[listArticlesQuery.AuthorUsername].Where(article =>
                article.TagList.Any(tag => tag.Tag == listArticlesQuery.Tag)
            ).ToList();

            VerifyArticlesMatchExpectations(result, expectedArticles);
        }
        
        [Fact]
        public async Task GivenFilterByTagAndFavoritedBy_WhenListArticles_ThenReturnsMatchingArticles()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                FavoritedByUsername = _module.AuthenticatedUserUsername,
                Tag = _module.ExistingArticleTag1
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            
            var expectedArticles = _module.FavoritedArticles.Where(article =>
                article.TagList.Any(tag => tag.Tag == listArticlesQuery.Tag)
            ).ToList();

            VerifyArticlesMatchExpectations(result, expectedArticles);
        }
        
        [Fact]
        public async Task GivenFilterByAuthorAndFavoritedBy_WhenListArticles_ThenReturnsMatchingArticles()
        {
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                FavoritedByUsername = _module.AuthenticatedUserUsername,
                AuthorUsername = _module.UserArticles.Keys.First(username => username != _module.AuthenticatedUserUsername),
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            
            var expectedArticles = _module.UserArticles[listArticlesQuery.AuthorUsername].Where(article => article.Favorited).ToList();

            VerifyArticlesMatchExpectations(result, expectedArticles);
        }
    }
}