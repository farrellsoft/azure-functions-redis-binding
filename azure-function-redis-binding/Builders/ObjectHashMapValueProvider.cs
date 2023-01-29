using System;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
	public class ObjectHashMapValueProvider<TValue> : IValueProvider
	{
        private readonly string _connection;
        private readonly string _key;

        public ObjectHashMapValueProvider(string connectionName, string key, IConfiguration confiuguration)
        {
            if (connectionName == null)
                throw new ArgumentNullException("You must specify the name of the connection in settings");

            _connection = confiuguration.GetValue<string>(connectionName);
            if (_connection == null)
                throw new ArgumentNullException($"The settings value {connectionName} was not found");

            _key = key;
        }

        public Type Type => typeof(TValue);

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
                returnDictionary.Add(resultNames[idx], JsonConvert.DeserializeObject<TValue>(resultValues[idx]));
            }

            return returnDictionary;
        }

        public string ToInvokeString() => string.Empty;
    }
}

