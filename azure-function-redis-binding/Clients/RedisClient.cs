using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Clients
{
    [ExcludeFromCodeCoverage]
    internal sealed class RedisClient : IClient
    {
        public async Task<HashEntry[]> GetHashEntries(string connectionString, string key)
        {
            using var connection = ConnectionMultiplexer.Connect(connectionString);
            var database = connection.GetDatabase();

            return await database.HashGetAllAsync(key);
        }

        public async Task<RedisValue[]> GetValues(string connectionString, string key)
        {
            using var connection = ConnectionMultiplexer.Connect(connectionString);
            var database = connection.GetDatabase();

            return await database.ListRangeAsync(key);
        }

        public async Task<string?> GetString(string connectionString, string key)
        {
            using var connection = ConnectionMultiplexer.Connect(connectionString);
            var database = connection.GetDatabase();

            return  await database.StringGetAsync(key);
        }   
    }

    public interface IClient
	{
		Task<string?> GetString(string connectionString, string key);
        Task<RedisValue[]> GetValues(string connectionString, string key);
        Task<HashEntry[]> GetHashEntries(string connectionString, string key);
    }
}

