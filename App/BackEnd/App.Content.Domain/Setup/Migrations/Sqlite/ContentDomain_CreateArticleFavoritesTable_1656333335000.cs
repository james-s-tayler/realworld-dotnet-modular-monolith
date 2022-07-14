using App.Core.SchemaManagement;
using FluentMigrator;
using JetBrains.Annotations;

namespace App.Content.Domain.Setup.Migrations.Sqlite
{
    [Tags(DbConstants.SQLite)]
    [Migration(1656333335000)]
    [UsedImplicitly]
    public class ContentDomain_CreateArticleFavoritesTable_1656333335000 : Migration
    {
        public override void Up()
        {
            Create.Table("article_favorites")
                .WithColumn("article_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("user_id").AsInt32().NotNullable().PrimaryKey();

            Create.ForeignKey("fk_article_favorites_article_id")
                .FromTable("article_favorites")
                .ForeignColumn("article_id")
                .ToTable("articles")
                .PrimaryColumn("id");
            
            Create.ForeignKey("fk_article_favorites_user_id")
                .FromTable("article_favorites")
                .ForeignColumn("user_id")
                .ToTable("users")
                .PrimaryColumn("user_id");
        }

        public override void Down()
        {
            Delete.ForeignKey("fk_article_favorites_article_id");
            Delete.ForeignKey("fk_article_favorites_user_id");
            Delete.Table("article_favorites");
        }
    }
}