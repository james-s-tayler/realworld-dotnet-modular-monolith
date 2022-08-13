using System.Diagnostics.CodeAnalysis;
using App.Core.SchemaManagement;
using FluentMigrator;
using JetBrains.Annotations;

namespace App.Feed.Domain.Setup.Migrations.Sqlite
{
    [ExcludeFromCodeCoverage]
    [Tags(DbConstants.SQLite)]
    [Migration(1658397659000)]
    [UsedImplicitly]
    public class FeedDomain_CreateFeedTables_1658397659000 : Migration
    {
        public override void Up()
        {
            Create.Table("follows")
                .WithColumn("user_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("following_user_id").AsInt32().NotNullable().PrimaryKey();

            Create.Table("articles")
                .WithColumn("article_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("user_id").AsInt32().NotNullable()
                .WithColumn("created_at").AsString().NotNullable();

            Create.ForeignKey("fk_articles_user_id")
                .FromTable("articles")
                .ForeignColumn("user_id")
                .ToTable("follows")
                .PrimaryColumn("user_id");

            Create.ForeignKey("fk_articles_following_user_id")
                .FromTable("articles")
                .ForeignColumn("user_id")
                .ToTable("follows")
                .PrimaryColumn("following_user_id");
        }

        public override void Down()
        {
            Delete.ForeignKey("fk_articles_user_id");
            Delete.ForeignKey("fk_articles_following_user_id");
            Delete.Table("follows");
            Delete.Table("articles");
        }
    }
}