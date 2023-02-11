using System;
using Newtonsoft.Json;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Converters
{
    public class JsonValueConverter : IValueConverter
    {
        public TValue? GetObjectFromString<TValue>(string jsonString) where TValue : class
        {
            if (string.IsNullOrEmpty(jsonString))
                return null;

            return JsonConvert.DeserializeObject<TValue>(jsonString);
        }
    }

    public interface IValueConverter
    {
        TValue? GetObjectFromString<TValue>(string jsonString) where TValue : class;
    }
}

