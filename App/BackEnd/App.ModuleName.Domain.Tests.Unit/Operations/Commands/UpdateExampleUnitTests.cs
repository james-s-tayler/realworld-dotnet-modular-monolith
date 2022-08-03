using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using App.ModuleName.Domain.Contracts.DTOs;
using App.ModuleName.Domain.Contracts.Operations.Commands.UpdateExample;
using App.ModuleName.Domain.Tests.Unit.Setup;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.ModuleName.Domain.Tests.Unit.Operations.Commands
{
    [Collection(nameof(ModuleNameModuleTestCollection))]
    public class UpdateExampleUnitTests : UnitTestBase
    {
        private readonly ModuleNameModuleSetupFixture _module;

        public UpdateExampleUnitTests(ModuleNameModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenUpdateNonExistentExample_WhenUpdateExample_ThenValidationError()
        {
            //arrange
            var updateExampleCommand = new UpdateExampleCommand { ExampleInput = _module.AutoFixture.Create<ExampleDTO>() };

            //act
            var result = await _module.Mediator.Send(updateExampleCommand);

            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
        }

        [Fact]
        public async Task GivenNoExample_WhenUpdateExample_ThenValidationError()
        {
            //arrange
            var updateExampleCommand = new UpdateExampleCommand { ExampleInput = null };

            //act
            var result = await _module.Mediator.Send(updateExampleCommand);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
        }
    }
}