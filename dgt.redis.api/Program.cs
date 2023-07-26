using Cassandra;
using dgt.redis.api;
using dgt.redis.api.Persistence;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStackExchangeRedisCache(o =>
{
    o.Configuration = builder.Configuration.GetConnectionString("Redis");
    o.InstanceName = "RedisSDKReseau_";

});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure Redis connection
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis:6379"));

// Configure Cassandra connection
builder.Services.AddSingleton<IDatabaseRepository>(new CassandraRepository("cassandra", "my_keyspace"));

var cluster = Cluster.Builder()
            .AddContactPoint("127.0.0.1")
            .Build();
var session = cluster.Connect("your_keyspace");

// Initialize Cassandra
var initializer = new CassandraInitializer(session);
initializer.Initialize();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
