using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using App.Core.DataAccess;

namespace App.ModuleName.Domain.Contracts
{
    [ExcludeFromCodeCoverage]
    public class ModuleNameDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(ModuleNameDomainContracts).Assembly;
    }
}