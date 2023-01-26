using System;
using Microsoft.Azure.WebJobs;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
    public class RedisDictionaryBuilder<TValue> : IAsyncConverter<RedisAttribute, Dictionary<string, TValue>>
    {
        public async Task<Dictionary<string, TValue>> ConvertAsync(RedisAttribute input, CancellationToken cancellationToken)
        {
            return null;
        }

    }
}

