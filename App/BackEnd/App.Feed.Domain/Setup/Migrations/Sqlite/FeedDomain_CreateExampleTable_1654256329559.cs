using App.Core.SchemaManagement;
using FluentMigrator;
using JetBrains.Annotations;

namespace App.Feed.Domain.Setup.Migrations.Sqlite
{
    [Tags(DbConstants.SQLite)]
    [Migration(1654256329559)]
    [UsedImplicitly]
    public class FeedDomain_CreateExampleTable_1654256329559 : Migration
    {
        public override void Up()
        {
            Create.Table("example")
                .WithColumn("id").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("something").AsString(50).NotNullable().Unique();
        }

        public override void Down()
        {
            Delete.Table("example");
        }
    }
}