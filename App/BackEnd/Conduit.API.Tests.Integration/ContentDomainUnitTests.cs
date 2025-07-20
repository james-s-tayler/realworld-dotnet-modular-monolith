using System.Net;
using System.Threading.Tasks;
using App.Core.Testing;
using AutoFixture;
using Conduit.API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using Xunit;
using Xunit.Abstractions;

namespace Conduit.API.Tests.Integration
{
    [Collection(nameof(IntegrationTestCollection))]
    public class ContentDomainUnitTests : IntegrationTestBase
    {
        private readonly IConduitApiClient _unauthenticatedApiClient;
        private readonly IConduitApiClient _authenticatedApiClient;
        private readonly Fixture _autoFixture = new();
        private readonly NewUser _newUser;
        private string _token;

        public ContentDomainUnitTests(WebApplicationFactory<Program> applicationFactory, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var httpClient = applicationFactory.CreateClient();
            _unauthenticatedApiClient = RestService.For<IConduitApiClient>(httpClient);
            applicationFactory.ClearDatabaseTables();

            _newUser = new NewUser
            {
                Username = _autoFixture.Create<string>(),
                Email = $"{_autoFixture.Create<string>()}@{_autoFixture.Create<string>()}.com",
                Password = _autoFixture.Create<string>()
            };

            var response = _unauthenticatedApiClient.CreateUser(new NewUserRequest { User = _newUser }).GetAwaiter().GetResult();
            _token = response.Content!.User.Token;

            _authenticatedApiClient = RestService.For<IConduitApiClient>(applicationFactory.CreateDefaultClient(new TestAuthorizationDelegatingHandler(_token, "Token")));
        }

        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenPublishArticle_ThenArticleReturned()
        {
            //arrange
            var newArticleRequest = _autoFixture.Create<NewArticleRequest>();

            //act
            var response = await _authenticatedApiClient.CreateArticle(newArticleRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Should().NotBeNull();
            response.Content!.Article.Should().NotBeNull();
            response.Content!.Article.Title.Should().Be(newArticleRequest.Article.Title);
            response.Content!.Article.Description.Should().Be(newArticleRequest.Article.Description);
            response.Content!.Article.Body.Should().Be(newArticleRequest.Article.Body);
        }

        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenPublishArticle_ThenArticleContainsProfile()
        {
            //arrange
            var newArticleRequest = _autoFixture.Create<NewArticleRequest>();

            //act
            var response = await _authenticatedApiClient.CreateArticle(newArticleRequest);

            //assert
            response.Content!.Article.Author.Should().NotBeNull();
            response.Content!.Article.Author.Username.Should().Be(_newUser.Username);
            response.Content!.Article.Author.Following.Should().BeFalse();
        }

        [Fact]
        public async Task GivenAnUnauthenticatedUser_WhenPublishArticle_ThenNotAuthenticated()
        {
            //arrange
            var newArticleRequest = _autoFixture.Create<NewArticleRequest>();

            //act
            var response = await _unauthenticatedApiClient.CreateArticle(newArticleRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.Content.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenAnUnauthenticatedUser_WhenGetArticle_ThenArticleReturned()
        {
            //arrange
            var newArticleRequest = _autoFixture.Create<NewArticleRequest>();
            var response = await _authenticatedApiClient.CreateArticle(newArticleRequest);
            response.Should().NotBeNull();
            response.Content.Should().NotBeNull();
            var slug = response.Content!.Article.Slug;
            
            //act
            response = await _unauthenticatedApiClient.GetArticle(slug);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNull();
            response.Content!.Article.Should().NotBeNull();
            response.Content!.Article.Title.Should().Be(newArticleRequest.Article.Title);
            response.Content!.Article.Description.Should().Be(newArticleRequest.Article.Description);
            response.Content!.Article.Body.Should().Be(newArticleRequest.Article.Body);
        }
    }
}