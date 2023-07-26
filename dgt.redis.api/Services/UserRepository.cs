using dgt.redis.api.Models;
using dgt.redis.api.Persistence;
using Microsoft.Extensions.Caching.Distributed;

namespace dgt.redis.api.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IDistributedCache _cache;
        private readonly IDatabaseRepository _dbRepository; // Assume you have a separate repository for interacting with DB

        public UserRepository(IDistributedCache cache, IDatabaseRepository dbRepository)
        {
            _cache = cache;
            _dbRepository = dbRepository;
        }

        public async Task<List<User>?> GetUsersAsync()
        {
            var metadata = new CacheResponseMetadata();
            List<User>? users = await _cache.GetRecordAsync<List<User>?>(metadata.RecordKey);

            if (users == null)
            {
                users = await _dbRepository.GetUsersAsync();
                metadata.Message = $"Loaded from DB at {DateTime.Now}";
                metadata.LoadLocation = CacheLocation.Database;
                metadata.IsCachedData = false;
                // You probably want to store the retrieved users back to the cache here
            }
            else
            {
                metadata.LoadLocation = CacheLocation.Redis;
                metadata.IsCachedData = true;
            }

            return users;
        }
    }

}
