using System;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	internal class RedisHashMapValueProvider<TValue> : IValueProvider
	{
        private readonly string _connection;
        private readonly string _key;

        public RedisHashMapValueProvider(string connectionName, string key, IConfiguration configuration)
		{
            if (connectionName == null)
                throw new ArgumentNullException("You must specify the name of the connection in settings");

            _connection = configuration.GetValue<string>(connectionName);
            if (_connection == null)
                throw new ArgumentNullException($"The settings value {connectionName} was not found");

            _key = key;
        }

        public async Task<object> GetValueAsync()
        {
            using var connection = ConnectionMultiplexer.Connect(_connection);
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

            return returnDictionary;
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

