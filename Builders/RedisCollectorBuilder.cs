using Microsoft.Azure.WebJobs;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
    internal class RedisCollectorBuilder : IConverter<RedisAttribute, IAsyncCollector<RedisItem>>
    {
        public IAsyncCollector<RedisItem> Convert(RedisAttribute input)
        {
            return new RedisItemAsyncCollector(input.Key, input.Connection);
        }
    }

    internal class RedisItemAsyncCollector : IAsyncCollector<RedisItem>
    {
        private readonly string _key;
        private readonly string _connection;

        public RedisItemAsyncCollector(string key, string connection)
        {
            _key = key;
            _connection = connection;
        }

        public async Task AddAsync(RedisItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            using var connection = ConnectionMultiplexer.Connect(_connection);
            var db = connection.GetDatabase();

            await db.StringSetAsync(_key, item.Value);
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}