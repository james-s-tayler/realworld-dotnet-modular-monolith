using System.Reflection;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts
{
    public class ContentDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(ContentDomainContracts).Assembly;
    }
}