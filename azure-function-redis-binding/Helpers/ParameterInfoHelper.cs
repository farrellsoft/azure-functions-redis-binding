using System;
using System.Reflection;

namespace Farrellsoft.Azure.Functions.Extensions.Redis.Helpers
{
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
	}

	public interface IParameterInfoHelper
	{
		RedisAttribute GetRedisAttribute(ParameterInfo parameterInfo);
		Type[] GetGenericTypeArgs(ParameterInfo parameterInfo);
	}
}

