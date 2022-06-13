using Conduit.Core.SchemaManagement;
using FluentMigrator;
using JetBrains.Annotations;

namespace Conduit.Social.Domain.Migrations.Sqlite
{
    [Tags(DbConstants.SQLite)]
    [Migration(165506604900)]
    [UsedImplicitly]
    public class SocialDomain_CreateFollowingTable_165506604900 : Migration
    {
        public override void Up()
        {
            Create.Table("following")
                .WithColumn("user_id").AsInt32().NotNullable().Indexed()
                .WithColumn("following_user_id").AsInt32().NotNullable().Indexed();
        }

        public override void Down()
        {
            Delete.Table("following");
        }
    }
}