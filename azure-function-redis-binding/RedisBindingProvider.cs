using System;
using System.Reflection;
using System.Reflection.Metadata;
using Farrellsoft.Azure.Functions.Extensions.Redis.Bindings;
using Farrellsoft.Azure.Functions.Extensions.Redis.Builders;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
	internal class RedisBindingProvider : IBindingProvider
	{
        private readonly IConfiguration _configuration;

        public RedisBindingProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

		public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var attribute = context.Parameter.GetCustomAttribute<RedisAttribute>(inherit: false);
            var genericArgs = context.Parameter.ParameterType.GetGenericArguments();

            // we are dealing with a string
            if (genericArgs.Length == 0)
            {
                // todo: validate we are dealing with a string or T

                return Task.FromResult<IBinding>(new RedisItemBinding(attribute, _configuration, context.Parameter.ParameterType));
            }

            // we are dealing with a destination type generic with one arg, it needs to be a list
            if (genericArgs.Length == 1)
            {
                // todo: validate we are dealing with a List<T>

                var providerType = typeof(RedisListBinding<>);
                var constructedProvider = providerType.MakeGenericType(new[] { genericArgs[0] });
                return Task.FromResult<IBinding>((IBinding)Activator.CreateInstance(
                    type: constructedProvider,
                    attribute,
                    _configuration));
            }

            if (genericArgs.Length == 2)
            {
                // todo: validate we are dealing with a Dictionary<string, T>

                var providerType = typeof(RedisHashMapBinding<>);
                var constructedProvider = providerType.MakeGenericType(new[] { genericArgs[1] });
                return Task.FromResult<IBinding>((IBinding)Activator.CreateInstance(
                    type: constructedProvider,
                    attribute,
                    _configuration));
            }

            throw new NotSupportedException("Binding types with more than 2 generics are not supported. See documentation for full list");
        }
    }
}

