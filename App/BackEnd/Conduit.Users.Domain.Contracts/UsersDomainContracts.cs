using System.Reflection;
using Application.Core.DataAccess;

namespace Conduit.Users.Domain.Contracts
{
    public class UsersDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(UsersDomainContracts).Assembly;
    }
}