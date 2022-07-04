using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Content.Domain.Infrastructure.Services;
using Application.Core.Modules;
using Application.Core.Testing;
using Application.Content.Domain.Setup.Module;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Social.Domain.Contracts.DTOs;
using Application.Social.Domain.Contracts.Operations.Queries.GetProfile;
using Microsoft.Extensions.DependencyInjection;
using AutoFixture;
using Moq;

namespace Application.Content.Domain.Tests.Unit.Setup
{
    public class ContentModuleSetupFixture : AbstractModuleSetupFixture
    {
        internal ArticleEntity ExistingNonFavoritedArticleEntity { get; private set; }
        internal ArticleEntity ExistingFavoritedArticleEntity { get; private set; }
        internal string ExistingArticleTag1 { get; } = "Tag1";
        internal string ExistingArticleTag2 { get; } = "Tag2";

        internal IUserRepository UserRepository { get; private set; }
        internal IArticleRepository ArticleRepository { get; private set; }
        internal Mock<ISocialService> SocialService { get; } = new ();
        

        protected override void AddConfiguration(IDictionary<string, string> configuration)
        {
        }

        protected override void ReplaceServices(AbstractModule module)
        {
            module.ReplaceTransient(SocialService.Object);
        }

        protected override void SetupPostProcess(ServiceProvider provider)
        {
            UserRepository = provider.GetService<IUserRepository>();
            ArticleRepository = provider.GetService<IArticleRepository>();

            var existingUserProfile = new ProfileDTO
            {
                Username = AuthenticatedUserUsername,
                Bio = AuthenticatedUserBio,
                Image = AuthenticatedUserImage,
                Following = true
            };
            
            SocialService
                .Setup(service => 
                    service.GetProfile(It.Is<string>(s => s.Equals(AuthenticatedUserUsername))))
                .ReturnsAsync(OperationResponseFactory.Success(new GetProfileQueryResult { Profile = existingUserProfile }));
        }

        public override void PerTestSetup()
        {
            WithExistingUser().GetAwaiter().GetResult();
            WithUnfavoritedArticle().GetAwaiter().GetResult();
            WithFavoritedArticle().GetAwaiter().GetResult();
        }

        public async Task WithExistingUser()
        {
            await UserRepository!.Create(new UserEntity {
                UserId = AuthenticatedUserId,
                Username = AuthenticatedUserUsername
            });
        }

        public async Task WithUnfavoritedArticle()
        {
            var nonFavoritedArticleId = ArticleRepository!.Create(new ArticleEntity
            {
                Title = $"{AutoFixture.Create<string>()} {AutoFixture.Create<string>()}",
                Description = AutoFixture.Create<string>(),
                Body = AutoFixture.Create<string>(),
                TagList = new List<TagEntity> {new() { Tag = ExistingArticleTag1 }, new() { Tag = ExistingArticleTag2}},
                Author = new UserEntity { UserId = AuthenticatedUserId }
            }).GetAwaiter().GetResult();
            ExistingNonFavoritedArticleEntity = await ArticleRepository.GetById(nonFavoritedArticleId);
        }

        public async Task WithFavoritedArticle()
        {
            var favoritedArticle = new ArticleEntity
            {
                Title = $"{AutoFixture.Create<string>()} {AutoFixture.Create<string>()}",
                Description = AutoFixture.Create<string>(),
                Body = AutoFixture.Create<string>(),
                TagList = new List<TagEntity> {new() { Tag = ExistingArticleTag1 }, new() { Tag = ExistingArticleTag2 }},
                Author = new UserEntity { UserId = AuthenticatedUserId }
            };
            var favoritedArticleId = await ArticleRepository!.Create(favoritedArticle);
            await ArticleRepository.FavoriteArticle(favoritedArticle.GetSlug());
            ExistingFavoritedArticleEntity = await ArticleRepository.GetById(favoritedArticleId);
        }

        public ContentModuleSetupFixture() : base(new ContentModule())
        {
        }
    }
}