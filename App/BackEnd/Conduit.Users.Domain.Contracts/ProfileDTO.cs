using Conduit.Core.DataAccess;

namespace Conduit.Users.Domain.Contracts
{
    public class ProfileDTO : ContractModel
    {
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public bool Following { get; set; }
    }
}