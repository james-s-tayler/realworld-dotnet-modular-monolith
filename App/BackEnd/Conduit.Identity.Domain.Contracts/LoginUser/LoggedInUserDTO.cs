namespace Conduit.Identity.Domain.Contracts.LoginUser
{
    public class LoggedInUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}