using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;

namespace App.Users.Domain.Contracts.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UpdateUserDTO : ContractModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
    }
}