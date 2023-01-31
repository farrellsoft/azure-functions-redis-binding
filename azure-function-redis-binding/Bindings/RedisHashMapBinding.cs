using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Configuration;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Bindings
{
	public class RedisHashMapBinding<TValue> : IBinding
	{
        private readonly RedisAttribute _attribute;
        private readonly IConfiguration _configuration;

		public RedisHashMapBinding(RedisAttribute attribute, IConfiguration configuration)
		{
            _attribute = attribute;
            _configuration = configuration;
		}

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            var providerType = typeof(RedisHashMapValueProvider<>);
            var constructedProvider = providerType.MakeGenericType(new[] { typeof(string), typeof(TValue) });
            return Task.FromResult<IValueProvider>((IValueProvider)Activator.CreateInstance(
                type: constructedProvider,
                _attribute.Connection,
                _attribute.Key,
                _configuration));
        }

        public Task<IValueProvider> BindAsync(BindingContext context) => throw new NotImplementedException();
        public bool FromAttribute => false;
        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor
        {
            Name = "HashMapParam",
            DisplayHints = new ParameterDisplayHints
            {
                Description = "Dictionary<string, T>"
            }
        };
    }
}

