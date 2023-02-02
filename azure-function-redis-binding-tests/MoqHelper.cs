using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests
{
	public static class MoqHelper
	{
		public static IConfiguration BuildConfiguration(IDictionary<string, string> values)
		{
			var configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(values);

			return configuration.Build();
		}

		public static RedisAttribute GetRedisAttribute(string key, string connectionName)
		{
			return new RedisAttribute(key) { Connection = connectionName };
		}

		public static ValueBindingContext GetValueBindingContext()
		{
			return new ValueBindingContext(
                new FunctionBindingContext(
                    functionInstance: new Mock<IFunctionInstanceEx>().Object,
                    functionCancellationToken: new System.Threading.CancellationToken()),
                cancellationToken: new System.Threading.CancellationToken());
        }
	}
}

