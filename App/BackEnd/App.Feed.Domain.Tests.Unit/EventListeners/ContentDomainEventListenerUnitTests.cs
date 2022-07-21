using System.Threading.Tasks;
using App.Content.Domain.Contracts.DTOs;
using App.Content.Domain.Contracts.Operations.Commands.PublishArticle;
using App.Core.Testing;
using App.Feed.Domain.Tests.Unit.Setup;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Feed.Domain.Tests.Unit.EventListeners
{
    [Collection(nameof(FeedModuleTestCollection))]
    public class ContentDomainEventListenerUnitTests : UnitTestBase
    {
        private readonly FeedModuleSetupFixture _module;

        public ContentDomainEventListenerUnitTests(FeedModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenPublishArticleEvent_WhenCheckArticleRepository_ThenArticleExists()
        {
            //arrange
            var publishArticleEvent = new PublishArticleCommandResult
            {
                UserId = _module.AuthenticatedUserId,
                ArticleId = _module.AutoFixture.Create<int>(),
                Article = _module.AutoFixture.Create<SingleArticleDTO>()
            };
            
            //act
            await _module.Mediator.Publish(publishArticleEvent);

            //assert
            var exists = await _module.ArticleRepository.Exists(publishArticleEvent.ArticleId);
            exists.Should().BeTrue();
        }
    }
}