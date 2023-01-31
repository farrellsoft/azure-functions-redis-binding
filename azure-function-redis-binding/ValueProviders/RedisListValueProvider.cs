using System;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	internal class RedisListValueProvider<TValue> : IValueProvider
	{
        private readonly string _connection;
        private readonly string _key;

        public RedisListValueProvider(string connectionName, string key, IConfiguration configuration)
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

            var redisResult = await database.ListRangeAsync(_key);
            if (typeof(TValue) != typeof(string))
            {
                return redisResult.Select(x => JsonConvert.DeserializeObject<TValue>(x.ToString())).ToList();
            }
            else
            {
                return redisResult.Select(x => x.ToString()).Cast<TValue>().ToList();
            }
        }

        public Type Type => typeof(List<TValue>);
        public string ToInvokeString() => string.Empty;
    }
}

