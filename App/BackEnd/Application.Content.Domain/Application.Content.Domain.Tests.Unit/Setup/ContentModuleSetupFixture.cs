using System.Collections.Generic;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Core.Modules;
using Application.Core.Testing;
using Application.Content.Domain.Setup.Module;
using Microsoft.Extensions.DependencyInjection;
using AutoFixture;

namespace Application.Content.Domain.Tests.Unit.Setup
{
    public class ContentModuleSetupFixture : AbstractModuleSetupFixture
    {
        internal ArticleEntity ExistingArticleEntity { get; private set; }
        internal string ExistingArticleTag1 { get; } = "Tag1";
        internal string ExistingArticleTag2 { get; } = "Tag2";

        internal IUserRepository UserRepository { get; private set; }
        internal IArticleRepository ArticleRepository { get; private set; }

        protected override void AddConfiguration(IDictionary<string, string> configuration)
        {
        }

        protected override void ReplaceServices(AbstractModule module)
        {
        }

        protected override void SetupPostProcess(ServiceProvider provider)
        {
            UserRepository = provider.GetService<IUserRepository>();
            ArticleRepository = provider.GetService<IArticleRepository>();
            var articleId = ArticleRepository!.Create(new ArticleEntity
            {
                Title = $"{AutoFixture.Create<string>()} {AutoFixture.Create<string>()}",
                Description = AutoFixture.Create<string>(),
                Body = AutoFixture.Create<string>(),
                TagList = new List<TagEntity> {new() { Tag = ExistingArticleTag1}, new() { Tag = ExistingArticleTag2}}
            }).GetAwaiter().GetResult();

            ExistingArticleEntity = ArticleRepository.GetById(articleId).GetAwaiter().GetResult();
        }

        public ContentModuleSetupFixture() : base(new ContentModule())
        {
        }
    }
}