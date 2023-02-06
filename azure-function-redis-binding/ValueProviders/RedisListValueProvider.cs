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
            return await _client.GetList<TValue>(_connectionName, _key);
        }

        public Type Type => typeof(List<TValue>);
        public string ToInvokeString() => string.Empty;
    }
}

