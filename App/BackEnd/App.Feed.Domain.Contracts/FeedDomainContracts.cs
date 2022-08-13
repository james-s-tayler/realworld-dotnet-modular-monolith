using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using App.Core.DataAccess;

namespace App.Feed.Domain.Contracts
{
    [ExcludeFromCodeCoverage]
    public class FeedDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(FeedDomainContracts).Assembly;
    }
}