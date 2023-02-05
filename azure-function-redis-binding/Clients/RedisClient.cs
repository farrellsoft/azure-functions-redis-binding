using System;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
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

        public Task<Dictionary<string, TReturn>> GetHashMap<TReturn>(string connectionName, string key)
        {
            throw new NotImplementedException();
        }

        public Task<List<TReturn>> GetList<TReturn>(string connectionName, string key)
        {
            throw new NotImplementedException();
        }

        public Task<TReturn> GetObject<TReturn>(string connectionName, string key)
        {
            throw new NotImplementedException();
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
        Task<TReturn> GetObject<TReturn>(string connectionName, string key);
        Task<List<TReturn>> GetList<TReturn>(string connectionName, string key);
        Task<Dictionary<string, TReturn>> GetHashMap<TReturn>(string connectionName, string key);
	}
}

