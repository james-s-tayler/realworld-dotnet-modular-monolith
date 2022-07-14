using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace App.Core.SchemaManagement
{
    public interface IDbCreator
    {
        void EnsureCreateDatabase(IConfiguration configuration, IHostEnvironment hostEnvironment, string moduleName);
    }
}