using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Users.Domain.Entities;
using Dapper;
using Dapper.Logging;

namespace Conduit.Users.Domain.Infrastructure.Repositories
{
    public class SqliteUserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SqliteUserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        //use sqlite - maybe even try litestream? https://news.ycombinator.com/item?id=31318708
        public async Task<bool> Exists(int id)
        {
            await using var connection = _connectionFactory.CreateConnection();
            
            string sql = "SELECT EXISTS(SELECT 1 FROM users WHERE id=@id)";
    
            var arguments = new
            {
                id = id
            };
            
            var exists = connection.ExecuteScalar<bool>(sql, arguments);
            
            return exists;
        }

        public async Task<User> GetById(int id)
        {
            await using var connection = _connectionFactory.CreateConnection();
            
            string sql = "SELECT * FROM users WHERE id=@id";
    
            var arguments = new
            {
                id = id
            };
            
            var user = connection.Query<User>(sql, arguments);
            
            return user.SingleOrDefault();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            await using var connection = _connectionFactory.CreateConnection();

            string sql = "SELECT * FROM users";

            return connection.Query<User>(sql);
        }

        public async Task<int> Create(User user)
        {
            await using var connection = _connectionFactory.CreateConnection();

            var sql = "INSERT INTO users (username, email, password, image, bio) VALUES (@username, @email, @password, @image, @bio)";

            var arguments = new
            {
                username = user.Username,
                email = user.Email,
                password = user.Password,
                image = user.Image,
                bio = user.Bio
            };

            return connection.Execute(sql, arguments);
        }

        public async Task Update(User user)
        {
            await using var connection = _connectionFactory.CreateConnection();

            var sql = "UPDATE users SET username = @username, email = @email, password = @password, image = @image, bio = @bio WHERE id = @id";

            var arguments = new
            {
                id = user.Id,
                username = user.Username,
                email = user.Email,
                password = user.Password,
                image = user.Image,
                bio = user.Bio
            };
            
            connection.Execute(sql, arguments);
        }

        public async Task Delete(int id)
        {
            await using var connection = _connectionFactory.CreateConnection();

            var sql = "DELETE FROM users WHERE id = @id";

            var arguments = new
            {
                id = id
            };

            connection.Execute(sql, arguments);
        }

        public async Task<User> GetByEmail(string email)
        {
            await using var connection = _connectionFactory.CreateConnection();

            var sql = "SELECT * FROM users WHERE email = @email";

            var arguments = new { email };
            
            return connection.Query<User>(sql, arguments).SingleOrDefault();
        }

        public async Task<bool> ExistsByUsername(string username)
        {
            await using var connection = _connectionFactory.CreateConnection();

            var sql = "SELECT EXISTS(SELECT 1 FROM users WHERE username=@username)";

            var arguments = new { username };
            
            return connection.ExecuteScalar<bool>(sql, arguments);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            await using var connection = _connectionFactory.CreateConnection();

            var sql = "SELECT EXISTS(SELECT 1 FROM users WHERE email=@email)";

            var arguments = new { email };
            
            return connection.ExecuteScalar<bool>(sql, arguments);
        }
    }
}