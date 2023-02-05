using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	public sealed class RedisListValueProvider<TValue> : IValueProvider
	{
        private readonly IClient _client;
        private readonly string _connectionName;
        private readonly string _key;

        public RedisListValueProvider(string connectionName, string key, IClient client)
		{
            _connectionName = connectionName;
            _key = key;
            _client = client;
        }

        public async Task<object> GetValueAsync()
        {
            /*using var connection = ConnectionMultiplexer.Connect(_connection);
            var database = connection.GetDatabase();

            var redisResult = await database.ListRangeAsync(_key);
            if (typeof(TValue) != typeof(string))
            {
                return redisResult.Select(x => JsonConvert.DeserializeObject<TValue>(x.ToString())).ToList();
            }
            else
            {
                return redisResult.Select(x => x.ToString()).Cast<TValue>().ToList();
            }*/
            return await _client.GetList<TValue>(_connectionName, _key);
        }

        public Type Type => typeof(List<TValue>);
        public string ToInvokeString() => string.Empty;
    }
}

