using Conduit.Core.SchemaManagement;
using FluentMigrator;
using JetBrains.Annotations;

namespace Conduit.Social.Domain.Migrations.Sqlite
{
    [Tags(DbConstants.SQLite)]
    [Migration(165506604900)]
    [UsedImplicitly]
    public class SocialDomain_CreateFollowersTable_165506604900 : Migration
    {
        public override void Up()
        {
            Create.Table("followers")
                .WithColumn("user_id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("follow_user_id").AsInt32().NotNullable().PrimaryKey();
        }

        public override void Down()
        {
            Delete.Table("followers");
        }
    }
}