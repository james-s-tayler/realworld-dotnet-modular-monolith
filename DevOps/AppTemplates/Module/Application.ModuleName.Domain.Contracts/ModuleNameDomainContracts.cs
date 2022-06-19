using System.Reflection;
using Application.Core.DataAccess;

namespace Application.ModuleName.Domain.Contracts
{
    public class ModuleNameDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(ModuleNameDomainContracts).Assembly;
    }
}