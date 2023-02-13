using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Converters
{
	public class RedisValueConverter : IRedisValueConverter
	{
		private readonly IClient _client;
		private readonly IConfiguration _configuration;
        private readonly IValueConverter _valueConverter;

		public RedisValueConverter(IClient client, IConfiguration configuration, IValueConverter valueConverter)
		{
			_client = client;
			_configuration = configuration;
            _valueConverter = valueConverter;
		}

        public async Task<Dictionary<string, TValue>> GetDictionary<TValue>(string connectionName, string key) where TValue : class
        {
            var connectionString = GetConnectionString(connectionName);
            var hashEntries = await _client.GetHashEntries(connectionString, key);

            var resultNames = hashEntries.Select(x => x.Name.ToString()).ToArray();
            var resultValues = hashEntries.Select(x => x.Value.ToString()).Cast<string>().ToArray();
            var returnDictionary = new Dictionary<string, TValue>();

            for (int idx = 0; idx < resultNames.Length; idx++)
            {
                if (typeof(TValue) != typeof(string))
                {
                    returnDictionary.Add(resultNames[idx], _valueConverter.GetObjectFromString<TValue>(resultValues[idx].ToString()));
                }
                else
                {
                    returnDictionary.Add(resultNames[idx], (TValue)(object)resultValues[idx].ToString());
                }
            }

            return returnDictionary;
        }

        public async Task<List<TValue>> GetList<TValue>(string connectionName, string key) where TValue : class
        {
            var values = await _client.GetValues(GetConnectionString(connectionName), key);
            if (typeof(TValue) != typeof(string))
            {
                return values.Select(x => _valueConverter.GetObjectFromString<TValue>(x.ToString())).ToList();
            }
            else
            {
                return values.Select(x => x.ToString()).Cast<TValue>().ToList();
            }
        }

        public async Task<TValue?> GetObject<TValue>(string connectionName, string key) where TValue : class
        {
            var stringValue = await _client.GetString(GetConnectionString(connectionName), key);
            if (string.IsNullOrEmpty(stringValue))
                return null;

            return _valueConverter.GetObjectFromString<TValue>(stringValue);
        }

        public async Task<string?> GetString(string connectionName, string key)
        {
            return await _client.GetString(GetConnectionString(connectionName), key);
        }

        string GetConnectionString(string connectionName)
        {
            if (string.IsNullOrEmpty(connectionName))
                throw new ArgumentNullException(nameof(connectionName));

            var connectionString = _configuration.GetValue<string>(connectionName);
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"The connection name {connectionName} does not map to a known configuration value");

            return connectionString;
        }
    }

	public interface IRedisValueConverter
	{
		Task<Dictionary<string, TValue>> GetDictionary<TValue>(string connectionName, string key) where TValue : class;
		Task<List<TValue>> GetList<TValue>(string connectionName, string key) where TValue : class;
		Task<TValue?> GetObject<TValue>(string connectionName, string key) where TValue : class;
		Task<string?> GetString(string connectionName, string key);
	}
}

