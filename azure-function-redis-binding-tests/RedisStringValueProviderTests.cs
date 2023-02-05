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
		public async void validate_for_a_given_valid_key_the_string_value_is_returned()
		{
			// arrange
			var clientMock = new Mock<IClient>();
			clientMock.Setup(x => x.GetStringValue(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync("valid value");

			var valueProvider = new RedisStringValueProvider("redisConnection", "valid_key", clientMock.Object);

			// act
			var result = await valueProvider.GetValueAsync();

			// assert
			Assert.Equal("valid value", result.ToString());
		}
	}
}

