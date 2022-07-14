using System.Reflection;
using App.Core.DataAccess;

namespace App.Social.Domain.Contracts
{
    public class SocialDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(SocialDomainContracts).Assembly;
    }
}