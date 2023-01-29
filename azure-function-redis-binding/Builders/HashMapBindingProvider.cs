using System;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Builders
{
	internal class HashMapBindingProvider : IBindingProvider
	{
        private readonly IConfiguration _configuration;

        public HashMapBindingProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

		public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var parameterType = context.Parameter.ParameterType;
            if (parameterType.IsGenericType == false)
                throw new NotSupportedException("You must bind this to a Dictionary<TKey, TValue>");

            var genericArgs = parameterType.GetGenericArguments();
            if (genericArgs.Length != 2)
                throw new NotSupportedException("You must bind this to a Dictionary<TKey, TValue>");

            // create with the second parameter type
            var attribute = context.Parameter.GetCustomAttribute<RedisHashAttribute>(inherit: false);
            return Task.FromResult<IBinding>(new HashMapBinding(attribute, genericArgs[1], _configuration));
        }
    }
}

