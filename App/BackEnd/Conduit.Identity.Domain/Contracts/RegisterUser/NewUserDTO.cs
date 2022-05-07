namespace Conduit.Domain.Identity.Contracts.RegisterUser
{
    public class NewUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}