using System.Reflection;
using Application.Core.DataAccess;

namespace Application.Content.Domain.Contracts
{
    public class ContentDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(ContentDomainContracts).Assembly;
    }
}