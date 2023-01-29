using System;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Configuration;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
	public class HashMapBinding : IBinding
	{
        private readonly Type _innerType;
        private readonly RedisHashAttribute _attribute;
        private readonly IConfiguration _configuration;

		public HashMapBinding(RedisHashAttribute attribute, Type innerType, IConfiguration configuration)
		{
            _innerType = innerType;
            _attribute = attribute;
            _configuration = configuration;
		}

        public bool FromAttribute => false;

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            if (_innerType.IsClass && _innerType != typeof(string))
            {
                // value assumed to be a class
                var dictionaryType = typeof(ObjectHashMapValueProvider<>);
                var constructedProvider = dictionaryType.MakeGenericType(new[] { _innerType });
                return Task.FromResult<IValueProvider>((IValueProvider)Activator.CreateInstance(
                    type: constructedProvider,
                    _attribute.Connection,
                    _attribute.Key,
                    _configuration));
            }

            if (_innerType.IsClass && _innerType == typeof(string))
            {
                // value assumed to be string
                return Task.FromResult<IValueProvider>(new StringHashMapValueProvider(_attribute.Connection, _attribute.Key, _configuration));
            }

            throw new NotSupportedException("HashMap value must be <string> or <TValue> where TValue is a class");
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => throw new NotImplementedException();

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor
        {
            Name = "MyParam",
            DisplayHints = new ParameterDisplayHints
            {
                Description = "HashMap"
            }
};
    }
}

