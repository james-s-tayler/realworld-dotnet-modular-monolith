using System.Diagnostics.CodeAnalysis;
using App.Core.SchemaManagement;
using FluentMigrator;
using JetBrains.Annotations;

namespace App.Content.Domain.Setup.Migrations.Sqlite
{
    [ExcludeFromCodeCoverage]
    [Tags(DbConstants.SQLite)]
    [Migration(1655893418000)]
    [UsedImplicitly]
    public class ContentDomain_CreateContentDbTables_1655893418000 : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("user_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("username").AsString().NotNullable().Unique();

            Create.Table("tags")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("tag").AsString().NotNullable().Unique();

            Create.Table("articles")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("user_id").AsInt32().NotNullable().Indexed()
                .WithColumn("title").AsString().NotNullable().Unique()
                .WithColumn("slug").AsString().NotNullable().Unique()
                .WithColumn("description").AsString().NotNullable()
                .WithColumn("body").AsString().NotNullable()
                .WithColumn("created_at").AsString().NotNullable()
                .WithColumn("updated_at").AsString().NotNullable();

            Create.ForeignKey("fk_articles_user_id")
                .FromTable("articles")
                .ForeignColumn("user_id")
                .ToTable("users")
                .PrimaryColumn("user_id");

            Create.Table("comments")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("user_id").AsInt32().NotNullable().Indexed()
                .WithColumn("body").AsString().NotNullable()
                .WithColumn("created_at").AsString().NotNullable()
                .WithColumn("updated_at").AsString().NotNullable();

            Create.ForeignKey("fk_comments_user_id")
                .FromTable("comments")
                .ForeignColumn("user_id")
                .ToTable("users")
                .PrimaryColumn("user_id");

            Create.Table("article_tags")
                .WithColumn("article_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("tag_id").AsInt32().NotNullable().PrimaryKey();

            Create.ForeignKey("fk_article_tags_article_id")
                .FromTable("article_tags")
                .ForeignColumn("article_id")
                .ToTable("articles")
                .PrimaryColumn("id");

            Create.ForeignKey("fk_article_tags_tag_id")
                .FromTable("article_tags")
                .ForeignColumn("tag_id")
                .ToTable("tags")
                .PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.ForeignKey("fk_articles_user_id");
            Delete.ForeignKey("fk_article_tags_article_id");
            Delete.ForeignKey("fk_article_tags_tag_id");
            Delete.ForeignKey("fk_comments_user_id");
            Delete.Table("users");
            Delete.Table("tags");
            Delete.Table("articles");
            Delete.Table("comments");
        }
    }
}