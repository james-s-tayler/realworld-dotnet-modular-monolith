namespace Application.Users.Domain.Entities
{
    internal class UserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; } = "I work at statefarm";
    }
}