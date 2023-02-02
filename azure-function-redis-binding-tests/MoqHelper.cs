using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace azure_function_redis_binding_tests
{
	public static class MoqHelper
	{
		public static IConfiguration BuildConfiguration(IDictionary<string, string> values)
		{
			var configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(values);

			return configuration.Build();
		}
	}
}

