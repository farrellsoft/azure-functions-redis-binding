using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Moq;
using Xunit;

namespace Tests.Converters
{
	public class given_an_instance_of_RedisValueConverter_calling_GetString
    {
		[Fact]
		public async void validate_a_single_call_to_GetString_is_invoked_on_the_client()
		{
			// arrange
			var clientMock = new Mock<IClient>();
            var configuration = MoqHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "validConnectionName", "someConnectionString" }
            });

			var converterMock = new Mock<IValueConverter>();
			var converter = new RedisValueConverter(clientMock.Object, configuration, converterMock.Object);

			// act
			await converter.GetString("validConnectionName", "someKey");

			// assert
			clientMock.Verify(x => x.GetString("someConnectionString", "someKey"), Times.Once);
		}

		[Fact]
		public async void validate_if_no_connection_name_is_passed_an_ArgumentNullException_is_thrown()
		{
            // arrange
            var clientMock = new Mock<IClient>();
            var configuration = MoqHelper.BuildConfiguration(new Dictionary<string, string>());
			var converterMock = new Mock<IValueConverter>();
			var converter = new RedisValueConverter(clientMock.Object, configuration, converterMock.Object);

			// act
			// assert
			await Assert.ThrowsAsync<ArgumentNullException>(() => converter.GetString(string.Empty, "someKey"));
        }

		[Fact]
		public async void validate_if_the_given_connection_name_does_not_map_to_a_configuration_value_an_ArgumentException_is_thrown()
		{
            // arrange
            var clientMock = new Mock<IClient>();
            var configuration = MoqHelper.BuildConfiguration(new Dictionary<string, string>());
            var converterMock = new Mock<IValueConverter>();
            var converter = new RedisValueConverter(clientMock.Object, configuration, converterMock.Object);

			// act
			// assert
			await Assert.ThrowsAsync<ArgumentException>(() => converter.GetString("connectionName", "someKey"));
        }

		[Fact]
		public async void validate_the_value_from_the_client_is_returned_from_the_converter()
		{
			// arrange
			const string value = "someString";
            var clientMock = new Mock<IClient>();
			clientMock.Setup(x => x.GetString(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(value);

            var configuration = MoqHelper.BuildConfiguration(new Dictionary<string, string>()
			{
				{ "validConnectionName", "someConnectionString" }
			});
            var converterMock = new Mock<IValueConverter>();
            var converter = new RedisValueConverter(clientMock.Object, configuration, converterMock.Object);

			// act
			var result = await converter.GetString("validConnectionName", "someKey");

			// assert
			Assert.Equal(value, result);
        }
	}
}

