using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
    public class RedisEnumerableBuilder<TValue> : IAsyncConverter<RedisAttribute, List<TValue>>
    {
        public async Task<List<TValue>> ConvertAsync(RedisAttribute input, CancellationToken cancellationToken)
        {
            if (input.Connection == null)
                throw new System.ArgumentNullException(nameof(input.Connection));

            using var connection = ConnectionMultiplexer.Connect(input.Connection);
            var database = connection.GetDatabase();

            var redisResult = await database.ListRangeAsync(input.Key);
            if (typeof(TValue).IsClass && typeof(TValue) != typeof(string))
            {
                return redisResult.Select(x => JsonConvert.DeserializeObject<TValue>(x.ToString())).ToList();
            }
            else
            {
                return redisResult.Select(x => x.ToString()).Cast<TValue>().ToList();
            }
        }
    }
}