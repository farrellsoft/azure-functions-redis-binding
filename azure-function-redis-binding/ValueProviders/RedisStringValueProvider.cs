using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	public sealed class RedisStringValueProvider : IValueProvider
	{
        private readonly IClient _client;
        private readonly string _connectionName;
        private readonly string _key;

		public RedisStringValueProvider(string connectionName, string key, IClient client)
		{
            _client = client;
            _connectionName = connectionName;
            _key = key;
		}

        public async Task<object> GetValueAsync()
        {
            return await _client.GetStringValue(_connectionName, _key);
        }

        public Type Type => typeof(string);
        public string ToInvokeString() => string.Empty;
    }
}

