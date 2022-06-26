using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Users.Domain.Entities;

namespace Application.Users.Domain.Infrastructure.Repositories
{
    internal class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<int, UserEntity> _repo = new();
        private static readonly Random Random = new();

        public Task<bool> Exists(int id)
        {
            return Task.FromResult(_repo.ContainsKey(id));
        }

        public Task<UserEntity> GetByUsername(string username)
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

        public async Task<UserEntity> GetById(int id)
        {
            if (await Exists(id))
                return _repo[id];
            
            return default;
        }
        
        public Task<UserEntity> GetByEmail(string email)
        {
            return Task.FromResult(_repo.Values.SingleOrDefault(user => user.Email.Equals(email)));
        }

        public Task<IEnumerable<UserEntity>> GetAll()
        {
            return Task.FromResult(_repo.Values.ToList().AsEnumerable());
        }

        public Task<int> Create(UserEntity userEntity)
        {
            var id = Random.Next(1, Int32.MaxValue);
            userEntity.Id = id;
            _repo[userEntity.Id] = userEntity;
            return Task.FromResult(userEntity.Id);
        }

        public Task Update(UserEntity userEntity)
        {
            _repo[userEntity.Id] = userEntity;
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