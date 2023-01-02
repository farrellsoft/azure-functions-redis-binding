using System;
using Microsoft.Azure.WebJobs.Description;

namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
    [Binding]
    public class RedisAttribute : Attribute
    {
        [AutoResolve]
        public string Key { get; private set; }   

        [AppSetting(Default = "RedisConnectionString")]
        public string? Connection { get; set; }

        public RedisAttribute(string key)
        {
            Key = key;
        }
    }
}