using System;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders
{
	internal class RedisStringValueProvider : IValueProvider
	{
        private readonly string _connection;
        private readonly string _key;

		public RedisStringValueProvider(string connectionName, string key, IConfiguration configuration)
		{
            if (connectionName == null)
                throw new ArgumentNullException("You must specify the name of the connection in settings");

            _connection = configuration.GetValue<string>(connectionName);
            if (_connection == null)
                throw new ArgumentNullException($"The settings value {connectionName} was not found");

            _key = key;
		}

        public Type Type => typeof(Dictionary<string, string>);

        public async Task<object> GetValueAsync()
        {
            using var connection = ConnectionMultiplexer.Connect(_connection);
            var database = connection.GetDatabase();

            var result = await connection.GetDatabase().StringGetAsync(_key);
            return result.ToString();
        }

        public string ToInvokeString() => string.Empty;
    }
}

