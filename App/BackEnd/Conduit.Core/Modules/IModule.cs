using System.Reflection;

namespace Conduit.Core.Modules
{
    public interface IModule
    {
        Assembly GetModuleAssembly();
        string GetModuleName();
    }
}