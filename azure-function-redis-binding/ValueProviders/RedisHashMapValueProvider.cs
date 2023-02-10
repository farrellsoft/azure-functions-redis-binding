using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	public sealed class RedisHashMapValueProvider<TValue> : IValueProvider
	{
        private readonly IRedisValueConverter _valueConverter;
        private readonly string _connectionName;
        private readonly string _key;

        public RedisHashMapValueProvider(string connectionName, string key, IRedisValueConverter valueConverter)
		{
            _connectionName = connectionName;
            _key = key;
            _valueConverter = valueConverter;
        }

        public async Task<object> GetValueAsync()
        {
            return await _valueConverter.GetDictionary<TValue>(_connectionName, _key);
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

