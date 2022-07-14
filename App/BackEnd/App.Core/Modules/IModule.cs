using System;
using System.Reflection;

namespace App.Core.Modules
{
    public interface IModule
    {
        Assembly GetModuleAssembly();
        Assembly GetModuleContractsAssembly();
        string GetModuleName();
        Type GetModuleType();
    }
}