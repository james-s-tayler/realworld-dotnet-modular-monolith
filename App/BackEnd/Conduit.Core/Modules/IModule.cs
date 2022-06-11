using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

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