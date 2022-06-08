using Conduit.Core.SchemaManagement;
using FluentMigrator;

namespace Conduit.Social.Domain.Migrations.Sqlite
{
    [Tags(DbConstants.SQLite)]
    [Migration(1654256329559)]
    public class CreateUsersTable_1654256329559 : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id")
                .AsInt32()
                .NotNullable()
                .PrimaryKey();
        }

        public override void Down()
        {
            Delete.Table("users");
        }
    }
}