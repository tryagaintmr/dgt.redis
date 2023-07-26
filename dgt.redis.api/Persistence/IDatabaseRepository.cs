using dgt.redis.api.Models;

namespace dgt.redis.api.Persistence
{
    public interface IDatabaseRepository
    {
        Task<List<User>> GetUsersAsync();
    }
}
