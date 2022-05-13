namespace Conduit.Identity.Domain.Contracts.Commands.LoginUser
{
    public class UserCredentialsDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}