using System;

namespace Conduit.Core.Context
{
    public class TestUserContext : IUserContext
    {
        public TestUserContext(string token)
        {
            Token = token;
            IsAuthenticated = false;
        }
        
        public TestUserContext(int userId, string username, string email, string token)
        {
            IsAuthenticated = true; 
            UserId = userId;
            Username = username;
            Email = email;
            Token = token;
        }

        public bool IsAuthenticated { get; }
        public int UserId { get; } 
        public string Username { get; }
        public string Email { get; }
        public string Token { get; }
    }
}