using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	public sealed class RedisStringValueProvider : IValueProvider
	{
        private readonly IRedisValueConverter _valueConverter;
        private readonly string _connectionName;
        private readonly string _key;

		public RedisStringValueProvider(string connectionName, string key, IRedisValueConverter valueConverter)
		{
            _valueConverter = valueConverter;
            _connectionName = connectionName;
            _key = key;
		}

        public async Task<object> GetValueAsync()
        {
            return await _valueConverter.GetString(_connectionName, _key);
        }

        public Type Type => typeof(string);
        public string ToInvokeString() => string.Empty;
    }
}

