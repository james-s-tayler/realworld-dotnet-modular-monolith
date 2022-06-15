using Application.Core.SchemaManagement;
using FluentMigrator;
using JetBrains.Annotations;

namespace Conduit.Social.Domain.Migrations.Sqlite
{
    [Tags(DbConstants.SQLite)]
    [Migration(1654256329559)]
    [UsedImplicitly]
    public class SocialDomain_CreateUsersTable_1654256329559 : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("username").AsString(50).NotNullable().Unique()
                .WithColumn("image").AsString(2048).Nullable()
                .WithColumn("bio").AsString(4000).Nullable();
        }

        public override void Down()
        {
            Delete.Table("users");
        }
    }
}