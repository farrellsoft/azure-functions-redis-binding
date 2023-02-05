using System;
using System.Reflection;
using System.Reflection.Metadata;
using Farrellsoft.Azure.Functions.Extensions.Redis.Bindings;
using Farrellsoft.Azure.Functions.Extensions.Redis.Builders;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.Helpers;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
	public class RedisBindingProvider : IBindingProvider
	{
        private readonly IClient _client;
        private readonly IParameterInfoHelper _parameterInfoHelper;

        public RedisBindingProvider(IClient client, IParameterInfoHelper parameterInfoHelper)
        {
            _client = client;
            _parameterInfoHelper = parameterInfoHelper;
        }

		public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var attribute = _parameterInfoHelper.GetRedisAttribute(context.Parameter);
            var genericArgs = _parameterInfoHelper.GetGenericTypeArgs(context.Parameter);

            // we are dealing with a string
            if (genericArgs.Length == 0)
            {
                // todo: validate we are dealing with a string or T

                return Task.FromResult<IBinding>(new RedisItemBinding(attribute, _client, context.Parameter.ParameterType));
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
                    _client));
            }

            if (genericArgs.Length == 2)
            {
                // todo: validate we are dealing with a Dictionary<string, T>

                var providerType = typeof(RedisHashMapBinding<>);
                var constructedProvider = providerType.MakeGenericType(new[] { genericArgs[1] });
                return Task.FromResult<IBinding>((IBinding)Activator.CreateInstance(
                    type: constructedProvider,
                    attribute,
                    _client));
            }

            throw new NotSupportedException("Binding types with more than 2 generics are not supported. See documentation for full list");
        }
    }
}

