using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Moq;
using Xunit;

namespace Tests.ValueProviders
{
	public class given_an_instance_of_RedisStringValueProvider
	{
		[Fact]
		public async void validate_the_RedisStringValueProvider_calls_the_GetStringValue_method_one_time()
		{
			// arrange
			var converterMock = new Mock<IRedisValueConverter>();
			converterMock.Setup(x => x.GetString(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync("valid value");

			var valueProvider = new RedisStringValueProvider("redisConnection", "valid_key", converterMock.Object);

			// act
			await valueProvider.GetValueAsync();

			// assert
			converterMock.Verify(x => x.GetString("redisConnection", "valid_key"), Times.Once);
		}
	}
}

