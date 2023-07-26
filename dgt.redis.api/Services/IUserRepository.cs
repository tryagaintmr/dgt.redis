using dgt.redis.api.Models;

namespace dgt.redis.api.Services
{
    public interface IUserRepository
    {
        Task<List<User>?> GetUsersAsync();
    }
}