using dgt.redis.api.Models;
using dgt.redis.api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dgt.redis.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private IDistributedCache _cache;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IDistributedCache cache)
        {
            _logger = logger;
            _userRepository = userRepository;
            _cache = cache;
        }

        // GET: api/<HomeController>
        [HttpGet]
        public async Task<List<User>?> Get()
        {
            List<User>? users = new List<User>();
            var metadata = new CacheResponseMetadata();

            users = await _cache.GetRecordAsync<List<User>?>(metadata.RecordKey);

            if (users is null)
            {
                users = await _userRepository.GetUsersAsync();
                metadata.Message = $"Loaded from DB at {DateTime.Now}";
                metadata.LoadLocation = CacheLocation.Database;
                metadata.IsCachedData = false;
            }
            else
            {
                metadata.LoadLocation = CacheLocation.Redis;
                metadata.IsCachedData = true;
            }

            return users;
        }

        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HomeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
