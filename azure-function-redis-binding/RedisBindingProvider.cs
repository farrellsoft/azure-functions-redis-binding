using System;
using System.Reflection;
using System.Reflection.Metadata;
using Farrellsoft.Azure.Functions.Extensions.Redis.Bindings;
using Farrellsoft.Azure.Functions.Extensions.Redis.Builders;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Farrellsoft.Azure.Functions.Extensions.Redis.Helpers;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
	public class RedisBindingProvider : IBindingProvider
	{
        private readonly IRedisValueConverter _valueConverter;
        private readonly IParameterInfoHelper _parameterInfoHelper;

        public RedisBindingProvider(IRedisValueConverter valueConverter, IParameterInfoHelper parameterInfoHelper)
        {
            _valueConverter = valueConverter;
            _parameterInfoHelper = parameterInfoHelper;
        }

		public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var attribute = _parameterInfoHelper.GetRedisAttribute(context.Parameter);
            var genericArgs = _parameterInfoHelper.GetGenericTypeArgs(context.Parameter);

            // we are dealing with a string (or POCO)
            if (genericArgs.Length == 0)
            {
                var parameterType = _parameterInfoHelper.GetParameterType(context.Parameter);
                if (parameterType != typeof(string) && parameterType.IsClass == false)
                    throw new NotSupportedException($"The type {parameterType.ToString()} is not supported for binding");

                var providerType = typeof(RedisItemBinding<>);
                var constructedProvider = providerType.MakeGenericType(new[] { parameterType });
                return Task.FromResult<IBinding>((IBinding)Activator.CreateInstance(
                    type: constructedProvider,
                    attribute,
                    _valueConverter));
            }

            // we are dealing with a destination type generic with one arg, it needs to be a list
            if (genericArgs.Length == 1)
            {
                var parameterType = _parameterInfoHelper.GetParameterType(context.Parameter);
                if (parameterType.GetGenericTypeDefinition() != typeof(List<>)) 
                    throw new NotSupportedException($"You may only use List<T> as the destination type when binding a single generic type");

                // todo: validate the inner argument is a string or object

                var providerType = typeof(RedisListBinding<>);
                var constructedProvider = providerType.MakeGenericType(new[] { genericArgs[0] });
                return Task.FromResult<IBinding>((IBinding)Activator.CreateInstance(
                    type: constructedProvider,
                    attribute,
                    _valueConverter));
            }

            if (genericArgs.Length == 2)
            {
                var parameterType = _parameterInfoHelper.GetParameterType(context.Parameter);
                if (parameterType.GetGenericTypeDefinition() != typeof(Dictionary<,>))
                    throw new NotSupportedException($"You may only use Dictionary<TKey, TValue> when being a two generic type");

                // todo: validate inner type is Object or string

                var providerType = typeof(RedisHashMapBinding<>);
                var constructedProvider = providerType.MakeGenericType(new[] { genericArgs[1] });
                return Task.FromResult<IBinding>((IBinding)Activator.CreateInstance(
                    type: constructedProvider,
                    attribute,
                    _valueConverter));
            }

            throw new NotSupportedException("Binding types with more than 2 generics are not supported. See documentation for full list");
        }
    }
}

