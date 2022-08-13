using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts
{
    [ExcludeFromCodeCoverage]
    public class ContentDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(ContentDomainContracts).Assembly;
    }
}