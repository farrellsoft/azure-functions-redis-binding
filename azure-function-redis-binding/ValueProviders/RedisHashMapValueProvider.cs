using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	public sealed class RedisHashMapValueProvider<TValue> : IValueProvider
	{
        private readonly IClient _client;
        private readonly string _connectionName;
        private readonly string _key;

        public RedisHashMapValueProvider(string connectionName, string key, IClient client)
		{
            _connectionName = connectionName;
            _key = key;
            _client = client;
        }

        public async Task<object> GetValueAsync()
        {
            return await _client.GetHashMap<TValue>(_connectionName, _key);
        }

        public Type Type
        {
            get
            {
                var dictionaryType = typeof(Dictionary<,>);
                var genericType = dictionaryType.MakeGenericType(new[] { typeof(string), typeof(TValue) });

                return genericType;
            }
        }
        public string ToInvokeString() => string.Empty;
    }
}

