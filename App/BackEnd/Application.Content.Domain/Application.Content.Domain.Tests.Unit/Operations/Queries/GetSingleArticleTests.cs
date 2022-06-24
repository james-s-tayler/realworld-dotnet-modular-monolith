using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.Content.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Content.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class GetSingleArticleTests : TestBase
    {
        private readonly ContentModuleSetupFixture _module;

        public GetSingleArticleTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenNoArticle_WhenGetArticle_ThenNotFound()
        {
            
            //arrange
            var getSingleArticleQuery = new GetSingleArticleQuery { Slug = "some-non-existent-article" };

            //act
            var result = await _module.Mediator.Send(getSingleArticleQuery);

            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
        }
    }
}