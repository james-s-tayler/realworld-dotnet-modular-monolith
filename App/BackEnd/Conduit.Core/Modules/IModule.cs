using System;
using System.Reflection;

namespace Conduit.Core.Modules
{
    public interface IModule
    {
        Assembly GetModuleAssembly();
        Assembly GetModuleContractsAssembly();
        string GetModuleName();
        Type GetModuleType();
    }
}