﻿using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Bindings
{
	public class RedisItemBinding : IBinding
	{
        private readonly RedisAttribute _attribute;
        private readonly IConfiguration _configuration;
        private readonly Type _targetType;

		public RedisItemBinding(RedisAttribute attribute, IConfiguration configuration, Type targetType)
		{
            _attribute = attribute;
            _configuration = configuration;
            _targetType = targetType;
		}

        public bool FromAttribute => false;

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            if (_targetType == typeof(string))
                return Task.FromResult<IValueProvider>(new RedisStringValueProvider(_attribute.Connection, _attribute.Key, _configuration));

            var providerType = typeof(RedisObjectValueProvider<>);
            var constructedProvider = providerType.MakeGenericType(new[] { _targetType });
            return Task.FromResult<IValueProvider>((IValueProvider)Activator.CreateInstance(
                type: constructedProvider,
                _attribute.Connection,
                _attribute.Key,
                _configuration));
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
