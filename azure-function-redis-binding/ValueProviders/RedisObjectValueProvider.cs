using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	public sealed class RedisObjectValueProvider<TValue> : IValueProvider
	{
        private readonly IClient _client;
        private readonly string _connectionName;
        private readonly string _key;

		public RedisObjectValueProvider(string connectionName, string key, IClient client)
		{
            _key = key;
            _connectionName = connectionName;
            _client = client;
        }

        public async Task<object> GetValueAsync()
        {
            /*using var connection = ConnectionMultiplexer.Connect(_connection);
            var database = connection.GetDatabase();

            var result = await connection.GetDatabase().StringGetAsync(_key);

            return JsonConvert.DeserializeObject<TValue>(result.ToString());*/
            return await _client.GetObject<TValue>(_connectionName, _key);
        }

        public Type Type => typeof(TValue);
        public string ToInvokeString() => string.Empty;
    }
}

