using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using Application.ModuleName.Domain.Contracts.Operations.Queries.GetExample;
using Application.ModuleName.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.ModuleName.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(ModuleNameModuleTestCollection))]
    public class GetExampleUnitTests : UnitTestBase
    {
        private readonly ModuleNameModuleSetupFixture _module;
        
        public GetExampleUnitTests(ModuleNameModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
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