using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Queries.GetTags;
using App.Content.Domain.Tests.Unit.Setup;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Content.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class GetTagsUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;

        public GetTagsUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenExistingTags_WhenGetTags_ThenTagsReturned()
        {
            //arrange
            var getTagsQuery = new GetTagsQuery();

            //act
            var result = await _module.Mediator.Send(getTagsQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Tags.Should().BeEquivalentTo(new []{ _module.ExistingArticleTag1, _module.ExistingArticleTag2 });
        }
    }
}