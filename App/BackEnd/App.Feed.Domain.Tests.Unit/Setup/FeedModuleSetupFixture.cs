using System;
using System.Collections.Generic;
using App.Core.Modules;
using App.Core.Testing;
using App.Feed.Domain.Entities;
using App.Feed.Domain.Infrastructure.Repositories;
using App.Feed.Domain.Setup.Module;
using Microsoft.Extensions.DependencyInjection;
using AutoFixture;

namespace App.Feed.Domain.Tests.Unit.Setup
{
    public class FeedModuleSetupFixture : AbstractModuleSetupFixture
    {
        internal IFollowRepository FollowRepository { get; private set; }
        internal IArticleRepository ArticleRepository { get; private set; }

        internal List<FollowEntity> Follows { get; private set; }
        internal List<ArticleEntity> FollowedUserArticles { get; private set; }
        internal List<ArticleEntity> NonFollowedUserArticles { get; private set; }

        public FeedModuleSetupFixture() : base(new FeedModule()) {}
        
        protected override void AddConfiguration(IDictionary<string, string> configuration)
        {
            
        }
        
        protected override void ReplaceServices(AbstractModule module)
        {
            
        }
        
        protected override void SetupPostProcess(ServiceProvider provider)
        {
            FollowRepository = provider.GetService<IFollowRepository>();
            ArticleRepository = provider.GetService<IArticleRepository>();
        }

        public override void PerTestSetup()
        {
            Follows = new List<FollowEntity>();
            FollowedUserArticles = new List<ArticleEntity>();
            NonFollowedUserArticles = new List<ArticleEntity>();

            WithUserAndArticles(AuthenticatedUserId, true); //self
            WithUserAndArticles(AutoFixture.Create<int>(), true);
            WithUserAndArticles(AutoFixture.Create<int>(), true);
            WithUserAndArticles(AutoFixture.Create<int>(), false);
        }

        public void WithUserAndArticles(int followingUserId, bool following)
        {
            if (following)
            {
                var follow = new FollowEntity { UserId = AuthenticatedUserId, FollowingUserId = followingUserId };
                FollowRepository.Follow(follow);
                Follows.Add(follow);
            }

            for (var i = 0; i < 3; i++)
            {
                var article = new ArticleEntity
                {
                    ArticleId = AutoFixture.Create<int>(),
                    UserId = followingUserId,
                    CreatedAt = DateTime.UtcNow
                };
                
                ArticleRepository.Insert(article);

                if (following)
                {
                    FollowedUserArticles.Add(article);
                }
                else
                {
                    NonFollowedUserArticles.Add(article);
                }
            }
        }
    }
}