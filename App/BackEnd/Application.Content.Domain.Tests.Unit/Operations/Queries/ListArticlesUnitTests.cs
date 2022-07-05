using System;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using Application.Content.Domain.Contracts.Operations.Queries.ListArticles;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.Content.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Content.Domain.Tests.Unit.Operations.Queries
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class ListArticlesUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;

        public ListArticlesUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenNoArticles_WhenListArticles_ThenReturnsEmptyCollection()
        {
            
            //arrange
            _module.ClearModuleDatabaseTables();
            var listArticlesQuery = new ListArticlesQuery {};

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Articles.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GivenANegativeOffset_WhenListArticles_ThenInvalidRequest()
        {
            
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                Offset = -1
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task GivenAnInvalidLimit_WhenListArticles_ThenInvalidRequest(int limit)
        {
            
            //arrange
            var listArticlesQuery = new ListArticlesQuery
            {
                Limit = limit
            };

            //act
            var result = await _module.Mediator.Send(listArticlesQuery);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
        }
    }
}