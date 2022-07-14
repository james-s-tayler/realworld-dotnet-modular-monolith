using System.Reflection;
using App.Core.DataAccess;

namespace App.Users.Domain.Contracts
{
    public class UsersDomainContracts : ContractModel
    {
        public static readonly Assembly Assembly = typeof(UsersDomainContracts).Assembly;
    }
}