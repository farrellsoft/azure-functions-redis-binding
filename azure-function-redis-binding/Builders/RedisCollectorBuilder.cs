using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
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

            if (_valueType == RedisValueType.Collection)
            {
                await SaveCollectionItem(item, db);
            }
        }

        async Task SaveCollectionItem(TValue item, IDatabase database)
        {
            if (item is IRedisListItem)
            {
                // get all list items for the key
                await SaveRedisListItem((IRedisListItem)item, database);
            }
            else
            {
                if (typeof(TValue).IsClass)
                {
                    await database.ListRightPushAsync(_key, JsonConvert.SerializeObject(item));
                }
                else
                {
                    await database.ListRightPushAsync(_key, item.ToString());
                }
            }
        }

        async Task SaveRedisListItem(IRedisListItem item, IDatabase database)
        {
            var listItemsRaw = await database.ListRangeAsync(_key);

            // build the raw list into objects (or strings)
            var listItems = listItemsRaw.Select((x, index) => new
            {
                Index = index,
                Value = JsonConvert.DeserializeObject<TValue>(x)
            }).ToList();

            // attempt to find the item based on its given id
            var foundItem = listItems.FirstOrDefault(x => ((IRedisListItem)x.Value).Id == item.Id);
            if (foundItem != null)
            {
                await database.ListSetByIndexAsync(_key, foundItem.Index, JsonConvert.SerializeObject(item));
            }
            else
            {
                // the item was not found by its id - insert it
                await database.ListRightPushAsync(_key, JsonConvert.SerializeObject(item));
            }
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}