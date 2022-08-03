using System;
using System.Net;
using System.Threading.Tasks;
using App.Core.Testing;
using AutoFixture;
using Conduit.API.Models;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;

namespace Conduit.API.Tests.Integration
{
    [Collection(nameof(IntegrationTestCollection))]
    public class UsersDomainUnitTests : IntegrationTestBase
    {
        public IConduitApiClient ApiClient { get; }
        public Fixture AutoFixture { get; } = new();

        public UsersDomainUnitTests(WebApplicationFactory<Program> applicationFactory, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            applicationFactory.WithWebHostBuilder(builder =>
            {
                var runningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
                builder.UseEnvironment(runningInDocker ? "Docker" : "Development");
            });
            ApiClient = RestService.For<IConduitApiClient>(applicationFactory.CreateClient());
            applicationFactory.ClearDatabaseTables();
        }

        [Fact]
        public async Task GivenANewUser_WhenRegisterUser_ThenNewUserReturned()
        {
            var newUserRequest = new NewUserRequest
            {
                User = new NewUser
                {
                    Username = AutoFixture.Create<string>(),
                    Email = $"{AutoFixture.Create<string>()}@{AutoFixture.Create<string>()}.com",
                    Password = AutoFixture.Create<string>()
                }
            };

            var response = await ApiClient.CreateUser(newUserRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Should().NotBeNull();
            response.Content!.User.Should().NotBeNull();
            response.Content.User.Username.Should().Be(newUserRequest.User.Username);
            response.Content.User.Email.Should().Be(newUserRequest.User.Email);
            response.Content.User.Token.Should().NotBeEmpty().And.StartWith("ey");
        }
    }
}