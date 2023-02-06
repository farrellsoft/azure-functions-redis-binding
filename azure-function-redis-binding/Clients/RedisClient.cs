using System;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Clients
{
    public sealed class RedisClient : IClient
    {
        private readonly IConfiguration _configuration;

        public RedisClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Dictionary<string, TReturn>> GetHashMap<TReturn>(string connectionName, string key)
        {
            var connectionString = GetConnectionString(connectionName);
            using var connection = ConnectionMultiplexer.Connect(connectionString);
            var database = connection.GetDatabase();

            var results = await database.HashGetAllAsync(key);

            var resultNames = results.Select(x => x.Name.ToString()).ToArray();
            var resultValues = results.Select(x => x.Value.ToString()).Cast<string>().ToArray();
            var returnDictionary = new Dictionary<string, TReturn>();

            for (int idx = 0; idx < resultNames.Length; idx++)
            {
                if (typeof(TReturn) != typeof(string))
                {
                    returnDictionary.Add(resultNames[idx], JsonConvert.DeserializeObject<TReturn>(resultValues[idx].ToString()));
                }
                else
                {
                    returnDictionary.Add(resultNames[idx], (TReturn)(object)resultValues[idx].ToString());
                }
            }

            return returnDictionary;
        }

        public async Task<List<TReturn>> GetList<TReturn>(string connectionName, string key)
        {
            var connectionString = GetConnectionString(connectionName);
            using var connection = ConnectionMultiplexer.Connect(connectionString);
            var database = connection.GetDatabase();

            var redisResult = await database.ListRangeAsync(key);
            if (typeof(TReturn) != typeof(string))
            {
                return redisResult.Select(x => JsonConvert.DeserializeObject<TReturn>(x.ToString())).ToList();
            }
            else
            {
                return redisResult.Select(x => x.ToString()).Cast<TReturn>().ToList();
            }
        }

        public async Task<TReturn?> GetObject<TReturn>(string connectionName, string key) where TReturn : class
        {
            var stringResult = await GetStringValue(connectionName, key);
            if (string.IsNullOrEmpty(stringResult))
                return null;

            return JsonConvert.DeserializeObject<TReturn>(stringResult);
        }

        public async Task<string?> GetStringValue(string connectionName, string key)
        {
            var connectionString = GetConnectionString(connectionName);
            using var connection = ConnectionMultiplexer.Connect(connectionString);

            var database = connection.GetDatabase();
            var result = await database.StringGetAsync(key);
            return result.HasValue ? result.ToString() : null;
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

    public interface IClient
	{
		Task<string?> GetStringValue(string connectionName, string key);
        Task<TReturn> GetObject<TReturn>(string connectionName, string key) where TReturn : class;
        Task<List<TReturn>> GetList<TReturn>(string connectionName, string key);
        Task<Dictionary<string, TReturn>> GetHashMap<TReturn>(string connectionName, string key);
	}
}

