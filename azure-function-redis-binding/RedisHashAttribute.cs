using System;
using Microsoft.Azure.WebJobs.Description;

namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
    [Binding]
    public class RedisHashAttribute : Attribute, IRedisAttribute
	{
        [AutoResolve]
        public string Key { get; private set; }

        [AppSetting(Default = "RedisConnectionString")]
        public string? Connection { get; set; }

        public RedisHashAttribute(string key)
		{
            Key = key;
		}
	}
}

