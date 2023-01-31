using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Configuration;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Bindings
{
	internal class RedisStringBinding : IBinding
	{
        private readonly RedisAttribute _attribute;
        private readonly IConfiguration _configuration;

		public RedisStringBinding(RedisAttribute attribute, IConfiguration configuration)
		{
            _attribute = attribute;
            _configuration = configuration;
		}

        public bool FromAttribute => false;

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            return Task.FromResult<IValueProvider>(new StringValueProvider(_attribute.Connection, _attribute.Key, _configuration));
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) => throw new NotImplementedException();

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor
        {
            Name = "StringParam",
            DisplayHints = new ParameterDisplayHints
            {
                Description = "String"
            }
        };
    }
}

