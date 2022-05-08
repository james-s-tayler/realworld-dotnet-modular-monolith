using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Identity.Domain.Entities;

namespace Conduit.Identity.Domain.Interactions.Outbound.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<int, User> _repo = new();
        private static readonly Random Random = new();

        public Task<bool> Exists(int id)
        {
            return Task.FromResult(_repo.ContainsKey(id));
        }

        public async Task<User> GetById(int id)
        {
            if (await Exists(id))
                return _repo[id];
            
            return default;
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult(_repo.Values.ToList().AsEnumerable());
        }

        public Task<int> Create(User entity)
        {
            var id = Random.Next(1, Int32.MaxValue);
            _repo[id] = entity;
            return Task.FromResult(id);
        }

        public Task Update(User entity)
        {
            _repo[entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            _repo.Remove(id, out _);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByUsername(string username)
        {
            return Task.FromResult(_repo.Values.Any(user => user.Username.Equals(username)));
        }

        public Task<bool> ExistsByEmail(string email)
        {
            return Task.FromResult(_repo.Values.Any(user => user.Email.Equals(email)));
        }
    }
}