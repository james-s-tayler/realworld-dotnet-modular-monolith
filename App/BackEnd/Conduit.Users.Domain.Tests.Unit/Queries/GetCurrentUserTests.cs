using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Core.Context;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Configuration;
using Conduit.Identity.Domain.Contracts.Queries.GetCurrentUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using Conduit.Identity.Domain.Tests.Unit.Setup;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ScottBrady91.AspNetCore.Identity;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit.Queries
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class GetCurrentUserTests
    {
        private readonly UsersModuleSetupFixture _usersModule;
        
        public GetCurrentUserTests(UsersModuleSetupFixture usersModule)
        {
            _usersModule = usersModule;
        }

        [Fact]
        public async Task GivenAuthenticatedUser_WhenGetCurrentUser_ThenUserIsReturned()
        {
            //arrange
            _usersModule.WithDefaultUserContext();
            var getCurrentUserQuery = new GetCurrentUserQuery();

            //act
            var result = await _usersModule.Mediator.Send(getCurrentUserQuery);
            
            //assert
            Assert.True(result.Result == OperationResult.Success);
            var currentUser = result.Response.CurrentUser;
            Assert.NotNull(currentUser);
            //need to implement equality comparison between DTOs and Entities so we can just do Equals and have it update automatically without tests missing anything
            Assert.Equal(_usersModule.ExistingUser.Email, currentUser.Email);
            Assert.Equal(_usersModule.ExistingUser.Username, currentUser.Username);
            Assert.NotEmpty(currentUser.Token);
        }
        
        [Fact]
        public async Task GivenUnauthenticatedUser_WhenGetCurrentUser_ThenNotAuthenticated()
        {
            //arrange
            _usersModule.WithUnauthenticatedUserContext();
            var getCurrentUserQuery = new GetCurrentUserQuery();

            //act
            var result = await _usersModule.Mediator.Send(getCurrentUserQuery);
            
            //assert
            Assert.True(result.Result == OperationResult.NotAuthenticated);
            Assert.Null(result.Response);
        }
        
        [Fact]
        public async Task GivenNonExistentUser_WhenGetCurrentUser_ThenFailsValidation()
        {
            //arrange
            _usersModule.WithRandomUserContext();
            var getCurrentUserQuery = new GetCurrentUserQuery();

            //act
            var result = await _usersModule.Mediator.Send(getCurrentUserQuery);
            
            //assert
            Assert.True(result.Result == OperationResult.ValidationError);
            Assert.Null(result.Response);
        }
    }
}