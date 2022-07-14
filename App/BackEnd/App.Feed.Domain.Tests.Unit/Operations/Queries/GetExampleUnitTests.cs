using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using App.Feed.Domain.Contracts.Operations.Queries.GetExample;
using App.Feed.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Feed.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(FeedModuleTestCollection))]
    public class GetExampleUnitTests : UnitTestBase
    {
        private readonly FeedModuleSetupFixture _module;
        
        public GetExampleUnitTests(FeedModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenNoExample_WhenGetExample_ThenNotFound()
        {
            //arrange
            var getExampleQuery = new GetExampleQuery { Id = 1 };
            
            //act
            var result = await _module.Mediator.Send(getExampleQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.NotFound);
        }
    }
}