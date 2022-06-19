using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.ModuleName.Domain.Contracts.Operations.Queries.GetExample;
using Application.ModuleName.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.ModuleName.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(ModuleNameTestCollection))]
    public class GetExampleTests : TestBase
    {
        private readonly ModuleNameSetupFixture _module;
        
        public GetExampleTests(ModuleNameSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenNoExample_WhenGetExample_ThenNotFound()
        {
            //arrange
            var getExampleQuery = new GetExampleQuery {Id = 1};
            
            //act
            var result = await _module.Mediator.Send(getExampleQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
        }
    }
}