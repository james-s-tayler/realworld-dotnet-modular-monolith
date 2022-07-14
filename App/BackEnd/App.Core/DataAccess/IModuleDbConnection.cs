using System.Data.Common;
using App.Core.SchemaManagement;

namespace App.Core.DataAccess
{
    public interface IModuleDbConnection
    {
        DbConnection Connection { get; }
        DbVendor Vendor { get; }
    }
}