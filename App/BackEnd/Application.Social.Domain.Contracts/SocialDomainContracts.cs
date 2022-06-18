using System.Reflection;
using Application.Core.DataAccess;

namespace Application.Social.Domain.Contracts
{
    public class SocialDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(SocialDomainContracts).Assembly;
    }
}