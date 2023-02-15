using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Helpers
{
    [ExcludeFromCodeCoverage]
    public class ParameterInfoHelper : IParameterInfoHelper
	{
		public RedisAttribute GetRedisAttribute(ParameterInfo parameterInfo)
		{
            return parameterInfo.GetCustomAttributes().OfType<RedisAttribute>().First();
        }

		public Type[] GetGenericTypeArgs(ParameterInfo parameterInfo)
		{
            return parameterInfo.ParameterType.GetGenericArguments();
        }

        public Type GetParameterType(ParameterInfo parameterInfo)
        {
			return parameterInfo.ParameterType;
        }
    }

	public interface IParameterInfoHelper
	{
		RedisAttribute GetRedisAttribute(ParameterInfo parameterInfo);
		Type[] GetGenericTypeArgs(ParameterInfo parameterInfo);
		Type GetParameterType(ParameterInfo parameterInfo);
	}
}

