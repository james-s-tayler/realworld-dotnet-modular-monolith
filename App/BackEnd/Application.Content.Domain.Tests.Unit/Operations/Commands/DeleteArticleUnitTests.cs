using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.DeleteArticle;
using Application.Content.Domain.Tests.Unit.Setup;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Content.Domain.Tests.Unit.Operations.Commands
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class DeleteArticleUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;
        private readonly DeleteArticleCommand _deleteArticleCommand;

        public DeleteArticleUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _deleteArticleCommand = new DeleteArticleCommand { Slug = _module.ExistingFavoritedArticleEntity.GetSlug() };
        }

        [Fact]
        public async Task GivenAnExistingArticle_WhenDeleteArticle_ThenArticleDeleted()
        {
            //pre-assert
            
            //act
            var result = await _module.Mediator.Send(_deleteArticleCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
        }
        
        [Fact]
        public async Task GivenSlugIsMissing_WhenFavoriteArticle_ThenInvalidRequest()
        {
            //arrange
            _deleteArticleCommand.Slug = null;

            //act
            var result = await _module.Mediator.Send(_deleteArticleCommand);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
    }
}