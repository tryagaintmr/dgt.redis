using dgt.redis.api.Models;
using Cassandra;
using ISession = Cassandra.ISession;

namespace dgt.redis.api
{
    public class CassandraInitializer
    {
        private readonly ISession _session;

        public CassandraInitializer(ISession session)
        {
            _session = session;
        }

        public void Initialize()
        {
            CreateTable();
            SeedData();
        }

        private void CreateTable()
        {
            var createTableCql = @"CREATE TABLE IF NOT EXISTS Users (
                                firstname text,
                                lastname text,
                                PRIMARY KEY (firstname, lastname))";
            _session.Execute(createTableCql);
        }

        private void SeedData()
        {
            var users = new List<User> {
                                    new() { FirstName = "William", LastName = "Jackson" },
                                    new() { FirstName = "Maria", LastName = "Moody" },
                                    new() { FirstName = "Sarah", LastName = "King" },
                                    new() { FirstName = "Gregory", LastName = "Estrada" },
                                    new() { FirstName = "Juan", LastName = "Russell" },
                                    new() { FirstName = "James", LastName = "Bryant" },
                                    new() { FirstName = "Patrick", LastName = "Mullins" },
                                    new() { FirstName = "Sandra", LastName = "Fleming" },
                                    new() { FirstName = "Miguel", LastName = "Ramsey" },
                                    new() { FirstName = "Monica", LastName = "Howell" }
            };

            foreach (var user in users)
            {
                var insertCql = $"INSERT INTO Users (firstname, lastname) VALUES ('{user.FirstName}', '{user.LastName}')";
                _session.Execute(insertCql);
            }
        }
    }

}
