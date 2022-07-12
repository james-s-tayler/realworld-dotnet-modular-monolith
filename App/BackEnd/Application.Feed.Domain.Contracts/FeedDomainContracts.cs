using System.Reflection;
using Application.Core.DataAccess;

namespace Application.Feed.Domain.Contracts
{
    public class FeedDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(FeedDomainContracts).Assembly;
    }
}