using App.Core.SchemaManagement;
using FluentMigrator;
using JetBrains.Annotations;

namespace App.Content.Domain.Setup.Migrations.Sqlite
{
    [Tags(DbConstants.SQLite)]
    [Migration(1657192190000)]
    [UsedImplicitly]
    public class ContentDomain_UpdateCommentTableAddArticleIdColumn_1657192190000 : Migration
    {
        public override void Up()
        {
            Alter.Table("comments")
                .AddColumn("article_id").AsInt32().NotNullable().Indexed();

            Create.ForeignKey("fk_comments_article_id")
                .FromTable("comments")
                .ForeignColumn("article_id")
                .ToTable("articles")
                .PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.ForeignKey("fk_comments_article_id");
            Delete.Column("article_id").FromTable("comments");
        }
    }
}