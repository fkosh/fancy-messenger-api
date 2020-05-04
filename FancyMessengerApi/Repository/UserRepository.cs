using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FancyMessengerApi.Entities;

namespace FancyMessengerApi.Repository
{
    public class UserRepository
    {
        private readonly List<UserEntity> _users;

        public UserRepository()
        {
            _users = new List<UserEntity>();
        }

        public async Task<string> InsertOneAsync(UserEntity instance, CancellationToken ct)
        {
            return await Task<string>.Factory.StartNew(
                () => {
                    instance.Id = Guid.NewGuid().ToString();
                    
                    _users.Add(instance);
                
                    return instance.Id;
                },
                ct
            );
        }
        
        public async Task<UserEntity> FindOneAsync(string username, CancellationToken ct)
        {
            return await Task<UserEntity>.Factory.StartNew(
                () => _users.SingleOrDefault(user => user.Username == username), ct
            );
        }
        
        public async Task<IEnumerable<UserEntity>> FindAllAsync(CancellationToken ct)
        {
            return await Task<IEnumerable<UserEntity>>.Factory.StartNew(
                _users.ToArray, ct
            );
        }
    }
}