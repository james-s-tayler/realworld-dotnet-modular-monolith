using System;

namespace Application.Core.DataAccess.Dapper.Sqlite
{
    public class SqliteDateTimeHandler : SqliteTypeHandler<DateTime>
    {
        public override DateTime Parse(object value)
        {
            var dateTime = DateTime.Parse((string) value).ToUniversalTime();
            var utcDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            return utcDateTime;
        }
    }
}