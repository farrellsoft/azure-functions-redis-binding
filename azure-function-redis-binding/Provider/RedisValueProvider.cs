﻿using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Provider
{
	public class RedisValueProvider : IRedisValueProvider
	{
		private readonly IClient _client;
		private readonly IConfiguration _configuration;

		public RedisValueProvider(IClient client, IConfiguration configuration)
		{
			_client = client;
			_configuration = configuration;
		}

        public async Task<Dictionary<string, TValue>> GetDictionary<TValue>(string connectionName, string key)
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
                    returnDictionary.Add(resultNames[idx], JsonConvert.DeserializeObject<TValue>(resultValues[idx].ToString()));
                }
                else
                {
                    returnDictionary.Add(resultNames[idx], (TValue)(object)resultValues[idx].ToString());
                }
            }

            return returnDictionary;
        }

        public async Task<List<TValue>> GetList<TValue>(string connectionName, string key)
        {
            var values = await _client.GetValues(GetConnectionString(connectionName), key);
            if (typeof(TValue) != typeof(string))
            {
                return values.Select(x => JsonConvert.DeserializeObject<TValue>(x.ToString())).ToList();
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

            return JsonConvert.DeserializeObject<TValue>(stringValue);
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

	internal interface IRedisValueProvider
	{
		Task<Dictionary<string, TValue>> GetDictionary<TValue>(string connectionName, string key);
		Task<List<TValue>> GetList<TValue>(string connectionName, string key);
		Task<TValue?> GetObject<TValue>(string connectionName, string key) where TValue : class;
		Task<string?> GetString(string connectionName, string key);
	}
}

