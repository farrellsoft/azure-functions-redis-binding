using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Moq;
using Xunit;

namespace Tests
{
	public class given_an_instance_of_RedisStringValueProvider
	{
		[Fact]
		public async void validate_the_RedisStringValueProvider_calls_the_GetStringValue_method_one_time()
		{
			// arrange
			var clientMock = new Mock<IClient>();
			clientMock.Setup(x => x.GetStringValue(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync("valid value");

			var valueProvider = new RedisStringValueProvider("redisConnection", "valid_key", clientMock.Object);

			// act
			await valueProvider.GetValueAsync();

			// assert
			clientMock.Verify(x => x.GetStringValue("redisConnection", "valid_key"), Times.Once);
		}
	}
}

