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
            /*using var connection = ConnectionMultiplexer.Connect(_connection);
            var database = connection.GetDatabase();

            var results = await database.HashGetAllAsync(_key);

            var resultNames = results.Select(x => x.Name.ToString()).ToArray();
            var resultValues = results.Select(x => x.Value.ToString()).Cast<string>().ToArray();
            var returnDictionary = new Dictionary<string, TValue>();

            for (int idx = 0; idx < resultNames.Length; idx++)
            {
                if (typeof(TValue) != typeof(string))
                {
                    returnDictionary.Add(resultNames[idx], JsonConvert.DeserializeObject<TValue>(resultValues[idx].ToString()));
                }
                else
                {
                    returnDictionary.Add(resultNames[idx], (TValue)(object)resultValues[idx].ToString());
                }
            }

            return returnDictionary;*/
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

