using Microsoft.Azure.WebJobs;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
    internal class RedisCollectorBuilder<TValue> : IConverter<RedisAttribute, IAsyncCollector<TValue>>
    {
        public IAsyncCollector<TValue> Convert(RedisAttribute input)
        {
            return new RedisItemAsyncCollector<TValue>(input.Key, input.Connection);
        }
    }

    internal class RedisItemAsyncCollector<TValue> : IAsyncCollector<TValue>
    {
        private readonly string _key;
        private readonly string _connection;

        public RedisItemAsyncCollector(string key, string connection)
        {
            _key = key;
            if (string.IsNullOrWhiteSpace(connection))
                throw new ArgumentNullException(nameof(connection));

            _connection = connection;
        }

        public async Task AddAsync(TValue item, CancellationToken cancellationToken = default(CancellationToken))
        {
            using var connection = ConnectionMultiplexer.Connect(_connection);
            var db = connection.GetDatabase();

            
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}