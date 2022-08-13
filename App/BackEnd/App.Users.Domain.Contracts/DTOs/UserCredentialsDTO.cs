using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using Destructurama.Attributed;

namespace App.Users.Domain.Contracts.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UserCredentialsDTO : ContractModel
    {
        public string Email { get; set; }

        [NotLogged]
        public string Password { get; set; }
    }
}