using System.Diagnostics.CodeAnalysis;
using App.Core.SchemaManagement;
using FluentMigrator;
using JetBrains.Annotations;

namespace App.ModuleName.Domain.Setup.Migrations.Sqlite
{
    [ExcludeFromCodeCoverage]
    [Tags(DbConstants.SQLite)]
    [Migration(1654256329559)]
    [UsedImplicitly]
    public class ModuleNameDomain_CreateExampleTable_1654256329559 : Migration
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