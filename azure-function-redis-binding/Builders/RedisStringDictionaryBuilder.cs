using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using StackExchange.Redis;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
    internal class RedisStringDictionaryBuilder<TValue> : IAsyncConverter<RedisAttribute, Dictionary<string, TValue>>
    {
        public async Task<Dictionary<string, TValue>> ConvertAsync(RedisAttribute input, CancellationToken cancellationToken)
        {
            if (input.Connection == null)
                throw new System.ArgumentNullException(nameof(input.Connection));

            using var connection = ConnectionMultiplexer.Connect(input.Connection);
            var database = connection.GetDatabase();

            var results = await database.HashGetAllAsync(input.Key);

            var resultNames = results.Select(x => x.Name.ToString()).ToArray();
            var resultValues = results.Select(x => x.Value.ToString()).Cast<TValue>().ToArray();
            var returnDictionary = new Dictionary<string, TValue>();

            for (int idx=0; idx < resultNames.Length; idx++)
            {
                returnDictionary.Add(resultNames[idx], resultValues[idx]);
            }

            return returnDictionary;
        }
    }
}

