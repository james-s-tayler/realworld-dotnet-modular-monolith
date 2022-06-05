using System.Data;
using Dapper;

namespace Conduit.Core.DataAccess.Dapper.Sqlite
{
    //https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/dapper-limitations
    public abstract class SqliteTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        // Parameters are converted by Microsoft.Data.Sqlite
        public override void SetValue(IDbDataParameter parameter, T value) => parameter.Value = value;
    }
}