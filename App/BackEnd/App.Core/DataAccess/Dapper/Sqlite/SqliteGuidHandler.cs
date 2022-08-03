using System;

namespace App.Core.DataAccess.Dapper.Sqlite
{
    public class SqliteGuidHandler : SqliteTypeHandler<Guid>
    {
        public override Guid Parse(object value) => Guid.Parse(( string )value);
    }
}