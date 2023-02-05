using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Configuration;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Bindings
{
	public class RedisListBinding<TValue> : IBinding
    {
        private readonly RedisAttribute _attribute;
        private readonly IClient _client;

		public RedisListBinding(RedisAttribute attribute, IClient client)
		{
            _attribute = attribute;
            _client = client;
		}

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var providerType = typeof(RedisListValueProvider<>);
            var constructedProvider = providerType.MakeGenericType(new[] { typeof(TValue) });
            return Task.FromResult<IValueProvider>((IValueProvider)Activator.CreateInstance(
                type: constructedProvider,
                _attribute.Connection,
                _attribute.Key,
                _client));
        }

        public bool FromAttribute => false;
        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => throw new NotImplementedException();
        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor
        {
            Name = "ListParam",
            DisplayHints = new ParameterDisplayHints
            {
                Description = "List<T>"
            }
        };
    }
}

