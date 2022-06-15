using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Application.Core.SchemaManagement
{
    public interface IDbCreator
    {
        void EnsureCreateDatabase(IConfiguration configuration, IHostEnvironment hostEnvironment, string moduleName);
    }
}