using System;
using Microsoft.Azure.WebJobs.Description;

namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
    [Binding]
    public class RedisAttribute : Attribute
    {
        [AutoResolve]
        public string Key { get; private set; }   

        public RedisValueType ValueType { get; private set; }

        [AppSetting(Default = "RedisConnectionString")]
        public string? Connection { get; set; }

        public int TimeToLiveSeconds { get; set; }

        public RedisAttribute(string key)
        {
            Key = key;
        }

        public RedisAttribute(string key, RedisValueType valueType)
        {
            Key = key;
            ValueType = valueType;
        }
    }

    public enum RedisValueType
    {
        None,
        Single,
        Collection
    }
}