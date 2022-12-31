using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
    internal class RedisItemBuilder<TValue> : IAsyncConverter<RedisAttribute, TValue>
    {
        public async Task<TValue> ConvertAsync(RedisAttribute input, CancellationToken cancellationToken)
        {
            if (input.Connection == null)
                throw new System.ArgumentNullException(nameof(input.Connection));

            using var connection = ConnectionMultiplexer.Connect(input.Connection);
            var database = connection.GetDatabase();

            var redisResult = await database.StringGetAsync(input.Key);
            if (typeof(TValue).IsClass && typeof(TValue) != typeof(string))
            {
                return JsonConvert.DeserializeObject<TValue>(redisResult.ToString());
            }
            else
            {
                var array = new RedisValue[] { redisResult };
                return array.Select(x => x.ToString()).Cast<TValue>().First();
            }
        }
    }
}