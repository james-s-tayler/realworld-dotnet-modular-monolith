using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using App.Feed.Domain.Contracts.DTOs;
using App.Feed.Domain.Contracts.Operations.Queries.GetFeed;
using App.Feed.Domain.Tests.Unit.Setup;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Feed.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(FeedModuleTestCollection))]
    public class GetFeedUnitTests : UnitTestBase
    {
        private readonly FeedModuleSetupFixture _module;
        
        public GetFeedUnitTests(FeedModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenNoExample_WhenGetFeed_ThenValidationError()
        {
            //arrange
            var updateExampleQuery = new GetFeedQuery { ExampleInput = null };
            
            //act
            var result = await _module.Mediator.Send(updateExampleQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
        }
    }
}