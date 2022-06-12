using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Users.Domain.Entities;

namespace Conduit.Users.Domain.Infrastructure.Repositories
{
    internal class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<int, User> _repo = new();
        private static readonly Random Random = new();

        public Task<bool> Exists(int id)
        {
            return Task.FromResult(_repo.ContainsKey(id));
        }

        public Task<User> GetByUsername(string username)
        {
            return Task.FromResult(_repo.Values.SingleOrDefault(user => user.Username.Equals(username)));
        }

        public Task<bool> ExistsByUsername(string username)
        {
            return Task.FromResult(_repo.Values.Any(user => user.Username.Equals(username)));
        }

        public Task<bool> ExistsByEmail(string email)
        {
            return Task.FromResult(_repo.Values.Any(user => user.Email.Equals(email)));
        }

        public async Task<User> GetById(int id)
        {
            if (await Exists(id))
                return _repo[id];
            
            return default;
        }
        
        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(_repo.Values.SingleOrDefault(user => user.Email.Equals(email)));
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult(_repo.Values.ToList().AsEnumerable());
        }

        public Task<int> Create(User user)
        {
            var id = Random.Next(1, Int32.MaxValue);
            user.Id = id;
            _repo[user.Id] = user;
            return Task.FromResult(user.Id);
        }

        public Task Update(User user)
        {
            _repo[user.Id] = user;
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            _repo.Remove(id, out _);
            return Task.CompletedTask;
        }

        public Task<int> DeleteAll()
        {
            var count = _repo.Count;
            _repo.Clear();
            return Task.FromResult(count);
        }
    }
}