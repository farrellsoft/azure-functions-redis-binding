using Microsoft.Azure.WebJobs;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
    internal class RedisCollectorBuilder<TValue> : IConverter<RedisAttribute, IAsyncCollector<TValue>>
    {
        public IAsyncCollector<TValue> Convert(RedisAttribute input)
        {
            return new RedisItemAsyncCollector<TValue>(input.Key, input.Connection, input.ValueType);
        }
    }

    internal class RedisItemAsyncCollector<TValue> : IAsyncCollector<TValue>
    {
        private readonly string _key;
        private readonly string _connection;
        private readonly RedisValueType _valueType;

        public RedisItemAsyncCollector(string key, string connection, RedisValueType valueType)
        {
            _key = key;
            if (string.IsNullOrWhiteSpace(connection))
                throw new ArgumentNullException(nameof(connection));

            if (valueType == RedisValueType.None)
                throw new ArgumentException("Value type must be specified", nameof(valueType));

            _connection = connection;
            _valueType = valueType;
        }

        public async Task AddAsync(TValue item, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            using var connection = ConnectionMultiplexer.Connect(_connection);
            var db = connection.GetDatabase();

            // need to determine if the underlying type is a string or list
            if (_valueType == RedisValueType.Single)
            {
                await db.StringSetAsync(_key, item.ToString());
            }            
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}