using Conduit.Core.SchemaManagement;
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
                .WithColumn("id")
                .AsInt32()
                .NotNullable()
                .Unique();
        }

        public override void Down()
        {
            Delete.Table("users");
        }
    }
}