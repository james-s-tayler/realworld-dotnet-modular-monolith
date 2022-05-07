using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Identity.Domain.Entities;

namespace Conduit.Identity.Domain.Interactions.Outbound.Repositories
{
    public class PostgresUserRepository : IUserRepository
    {
        /*private readonly IDbConnectionFactory _dbConnectionFactory;

        public PostgresUserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }*/

        public Task<bool> Exists(int id)
        {
            throw new System.NotImplementedException();
        }
        
        public Task<bool> ExistsByUsername(string username)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ExistsByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> Create(User user)
        {
            throw new System.NotImplementedException();
            
            /*string commandText = "INSERT INTO users (username, password, email) VALUES (@username, @password @email)";
    
            var queryArguments = new
            {
                username = user.Username,
                password = user.Password,
                email = user.Email
            };
    
            //this is probs wrong - need to get the created object and return its id
            await using var connection = _dbConnectionFactory.CreateConnection();
            var userId = await connection.ExecuteAsync(commandText, queryArguments);
    
            return userId; */
        }

        public Task<User> Update(User entity)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}