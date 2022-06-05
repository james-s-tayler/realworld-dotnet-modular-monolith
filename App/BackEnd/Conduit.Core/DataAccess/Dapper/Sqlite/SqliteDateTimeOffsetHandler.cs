using System;

namespace Conduit.Core.DataAccess.Dapper.Sqlite
{
    public class SqliteDateTimeOffsetHandler : SqliteTypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value) => DateTimeOffset.Parse((string)value);
    }
}