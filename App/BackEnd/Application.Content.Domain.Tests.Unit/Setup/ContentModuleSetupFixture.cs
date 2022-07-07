using System;
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
        internal IDictionary<string, List<ArticleEntity>> UserArticles = new Dictionary<string, List<ArticleEntity>>();
        internal IDictionary<string, List<ArticleEntity>> TaggedArticles = new Dictionary<string, List<ArticleEntity>>();
        internal List<ArticleEntity> FavoritedArticles = new ();
        internal ArticleEntity ExistingNonFavoritedArticleEntity { get; private set; }
        internal ArticleEntity ExistingFavoritedArticleEntity { get; private set; }
        internal string ExistingArticleTag1 { get; } = "Tag1";
        internal string ExistingArticleTag2 { get; } = "Tag2";

        internal IUserRepository UserRepository { get; private set; }
        internal IArticleRepository ArticleRepository { get; private set; }
        internal Mock<ISocialService> SocialService { get; } = new ();
        
        public ContentModuleSetupFixture() : base(new ContentModule())
        {
        }

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
        }

        public override void PerTestSetup()
        {
            UserArticles.Clear();
            FavoritedArticles.Clear();
            TaggedArticles.Clear();
            SocialService.Reset();
            WithAuthenticatedUserEntityAndProfile().GetAwaiter().GetResult();
            WithUnfavoritedArticle().GetAwaiter().GetResult();
            WithFavoritedArticle().GetAwaiter().GetResult();

            WithUserAndArticles().GetAwaiter().GetResult();
            WithUserAndArticles().GetAwaiter().GetResult();
        }

        public async Task WithUserAndArticles()
        {
            var userId = await WithUserEntityAndProfile();
            
            await WithArticle(true, userId, new []{ ExistingArticleTag1 });
            await WithArticle(false, userId, new []{ ExistingArticleTag2 });
            await WithArticle(true, userId, Array.Empty<string>());
        }

        public async Task WithAuthenticatedUserEntityAndProfile()
        {
            await WithUserEntityAndProfile(AuthenticatedUserId, AuthenticatedUserUsername, AuthenticatedUserBio, AuthenticatedUserImage, true);
        }
        
        public async Task<int> WithUserEntityAndProfile()
        {
            var userId = AutoFixture.Create<int>();
            
            await WithUserEntityAndProfile(
                userId, 
                AutoFixture.Create<string>(), 
                AutoFixture.Create<string>(), 
                AutoFixture.Create<string>(),
                true);

            return userId;
        }
        
        public async Task WithUserEntityAndProfile(int userId, string username, string bio, string image, bool isFollowing)
        {
            await UserRepository!.Create(new UserEntity {
                UserId = userId,
                Username = username
            });
            
            var existingUserProfile = new ProfileDTO
            {
                Username = username,
                Bio = bio,
                Image = image,
                Following = isFollowing
            };

            var getProfileQueryResult = new GetProfileQueryResult { Profile = existingUserProfile };
            
            if (!UserContext.Object.IsAuthenticated)
            {
                getProfileQueryResult.Profile.Following = false;
            }
            
            SocialService
                .Setup(service => 
                    service.GetProfile(It.Is<string>(s => s.Equals(username))))
                .ReturnsAsync(OperationResponseFactory.Success(getProfileQueryResult));

            if (!UserArticles.ContainsKey(username))
            {
                UserArticles.Add(username, new List<ArticleEntity>());
            }
        }

        public async Task WithUnfavoritedArticle()
        {
            ExistingNonFavoritedArticleEntity = await WithArticle(false, AuthenticatedUserId, new []{ ExistingArticleTag2 });
        }

        public async Task WithFavoritedArticle()
        {
            ExistingFavoritedArticleEntity = await WithArticle(true, AuthenticatedUserId, new []{ ExistingArticleTag1 });
        }

        internal async Task<ArticleEntity> WithArticle(bool isFavorited, int authorId, string[] tags = default)
        {
            var article = new ArticleEntity
            {
                Title = $"{AutoFixture.Create<string>()} {AutoFixture.Create<string>()}",
                Description = AutoFixture.Create<string>(),
                Body = AutoFixture.Create<string>(),
                TagList = new List<TagEntity>(),
                Author = new UserEntity { UserId = authorId }
            };

            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    article.TagList.Add(new TagEntity { Tag = tag });
                }    
            }
            
            var articleId = await ArticleRepository!.Create(article);

            if (UserContext.Object.IsAuthenticated && isFavorited)
            {
                await ArticleRepository.FavoriteArticle(article.GetSlug());    
            }

            var author = await UserRepository.GetById(authorId);
            var createdArticle = await ArticleRepository.GetById(articleId);
            
            UserArticles[author.Username].Add(createdArticle);
            
            if (UserContext.Object.IsAuthenticated && isFavorited)
            {
                FavoritedArticles.Add(createdArticle);
            }

            foreach (var tag in createdArticle.TagList)
            {
                if (!TaggedArticles.ContainsKey(tag.Tag))
                {
                    TaggedArticles.Add(tag.Tag, new List<ArticleEntity>());
                }
                TaggedArticles[tag.Tag].Add(createdArticle);
            }
            
            return createdArticle;
        }
    }
}