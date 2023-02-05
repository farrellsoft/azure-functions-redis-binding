using System;
namespace Farrellsoft.Azure.Functions.Extensions.Redis.Clients
{
    public sealed class RedisClient : IClient
    {
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

        public Task<string> GetStringValue(string connectionName, string key)
        {
            throw new NotImplementedException();
        }
    }

    public interface IClient
	{
		Task<string> GetStringValue(string connectionName, string key);
        Task<TReturn> GetObject<TReturn>(string connectionName, string key);
        Task<List<TReturn>> GetList<TReturn>(string connectionName, string key);
        Task<Dictionary<string, TReturn>> GetHashMap<TReturn>(string connectionName, string key);
	}
}

