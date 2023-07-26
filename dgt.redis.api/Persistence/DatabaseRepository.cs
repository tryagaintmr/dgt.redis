using Cassandra;
using Cassandra.Mapping;
using dgt.redis.api.Models;

namespace dgt.redis.api.Persistence
{
    public class CassandraRepository : IDatabaseRepository
    {
        private readonly Cluster _cluster;

        public CassandraRepository(string contactPoint, string keyspace)
        {
            _cluster = Cluster.Builder().AddContactPoint(contactPoint).Build();
            Cassandra.ISession session = _cluster.Connect(keyspace);

            
            MappingConfiguration.Global.Define(
                new Map<User>()
                    .TableName("users") 
                    .Column(u => u.Id, cm => cm.WithName("id")) 
                    .Column(u => u.FirstName, cm => cm.WithName("firstname")) 
                    .Column(u => u.LastName, cm => cm.WithName("lastname")) 
            );
        }

        public async Task<List<User>> GetUsersAsync()
        {
            Cassandra.ISession session = _cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();
            IMapper mapper = new Mapper(session);

            
            return (List<User>)await mapper.FetchAsync<User>("SELECT * FROM users").ConfigureAwait(false);
        }
    }
}
