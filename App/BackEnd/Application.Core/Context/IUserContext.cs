namespace Application.Core.Context
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        int UserId { get; }
        string Username { get; }
        string Email { get; }
        string Token { get; }
    }
}