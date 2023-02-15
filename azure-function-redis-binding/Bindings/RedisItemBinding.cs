using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Bindings
{
	public class RedisItemBinding<TValue> : IBinding
	{
        private readonly RedisAttribute _attribute;
        private readonly IRedisValueConverter _valueConverter;

		public RedisItemBinding(RedisAttribute attribute, IRedisValueConverter valueConverter)
		{
            _attribute = attribute;
            _valueConverter = valueConverter;
		}

        public bool FromAttribute => false;

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            if (typeof(TValue) == typeof(string))
                return Task.FromResult<IValueProvider>(new RedisStringValueProvider(_attribute.Connection, _attribute.Key, _valueConverter));

            var providerType = typeof(RedisObjectValueProvider<>);
            var constructedProvider = providerType.MakeGenericType(new[] { typeof(TValue) });
            return Task.FromResult<IValueProvider>((IValueProvider)Activator.CreateInstance(
                type: constructedProvider,
                _attribute.Connection,
                _attribute.Key,
                _valueConverter));
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => throw new NotImplementedException();
        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor
        {
            Name = "ItemParam",
            DisplayHints = new ParameterDisplayHints
            {
                Description = "Item"
            }
        };
    }
}

