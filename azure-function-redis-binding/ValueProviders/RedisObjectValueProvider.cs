using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	public sealed class RedisObjectValueProvider<TValue> : IValueProvider where TValue : class
	{
        private readonly IRedisValueConverter _valueConverter;
        private readonly string _connectionName;
        private readonly string _key;

		public RedisObjectValueProvider(string connectionName, string key, IRedisValueConverter valueConverter)
		{
            _key = key;
            _connectionName = connectionName;
            _valueConverter = valueConverter;
        }

        public async Task<object> GetValueAsync()
        {
            return await _valueConverter.GetObject<TValue>(_connectionName, _key);
        }

        public Type Type => typeof(TValue);
        public string ToInvokeString() => string.Empty;
    }
}

