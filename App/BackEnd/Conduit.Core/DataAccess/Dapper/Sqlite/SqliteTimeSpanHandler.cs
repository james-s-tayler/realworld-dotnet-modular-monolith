using System;

namespace Conduit.Core.DataAccess.Dapper.Sqlite
{
    public class SqliteTimeSpanHandler : SqliteTypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value) => TimeSpan.Parse((string)value);
    }
}