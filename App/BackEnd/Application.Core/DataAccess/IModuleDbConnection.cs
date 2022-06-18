using System.Data.Common;
using Application.Core.SchemaManagement;

namespace Application.Core.DataAccess
{
    public interface IModuleDbConnection
    {
        DbConnection Connection { get; }
        DbVendor Vendor { get; }
    }
}