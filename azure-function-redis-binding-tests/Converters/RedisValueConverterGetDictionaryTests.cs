using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Moq;
using Xunit;

namespace Tests.Converters
{
	public class given_an_instance_of_RedisValueConverter_calling_GetDictionary
	{
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => converter.GetDictionary<TestObject>(string.Empty, "someKey"));
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
            await Assert.ThrowsAsync<ArgumentException>(() => converter.GetDictionary<TestObject>("connectionName", "someKey"));
        }

        [Fact]
		public async void validate_the_correct_number_of_values_are_returned_given_a_set_of_HashEntries()
		{
			// arrange
			var clientMock = new Mock<IClient>();
			clientMock.Setup(x => x.GetHashEntries(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new StackExchange.Redis.HashEntry[]
				{
					new StackExchange.Redis.HashEntry("firstName", "test"),
					new StackExchange.Redis.HashEntry("lastName", "user")
				});
			var configuration = MoqHelper.BuildConfiguration(new Dictionary<string, string>
			{
				{ "validConnectionName", "someConnectionString" }
			});

			var converterMock = new Mock<IValueConverter>();
			var converter = new RedisValueConverter(
				clientMock.Object,
				configuration,
				converterMock.Object);

			// act
			var result = await converter.GetDictionary<string>("validConnectionName", "someKey");

			// assert
			Assert.Equal(2, result.Values.Count);
		}

		[Fact]
		public async void validate_for_a_inner_type_of_string_no_call_to_GetObjectFromString_is_made()
		{
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetHashEntries(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new StackExchange.Redis.HashEntry[]
                {
                    new StackExchange.Redis.HashEntry("firstName", "test")
                });
            var configuration = MoqHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "validConnectionName", "someConnectionString" }
            });

            var converterMock = new Mock<IValueConverter>();
            var converter = new RedisValueConverter(
                clientMock.Object,
                configuration,
                converterMock.Object);

            // act
            var result = await converter.GetDictionary<string>("validConnectionName", "someKey");

			// assert
			converterMock.Verify(x => x.GetObjectFromString<It.IsAnyType>(It.IsAny<string>()), Times.Never);
        }

		[Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void validate_for_a_inner_type_is_a_class_the_right_number_of_calls_to_GetObjectFromString_are_made(int numberOfEntries)
        {
            // arrange
            var entries = new StackExchange.Redis.HashEntry[numberOfEntries];
            for (int i = 0; i < numberOfEntries; i++)
            {
                entries[i] = new StackExchange.Redis.HashEntry($"object{i}", "{}");
            }

            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetHashEntries(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(entries);
            var configuration = MoqHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "validConnectionName", "someConnectionString" }
            });

            var converterMock = new Mock<IValueConverter>();
            var converter = new RedisValueConverter(
                clientMock.Object,
                configuration,
                converterMock.Object);

            // act
            var result = await converter.GetDictionary<TestObject>("validConnectionName", "someKey");

            // assert
            converterMock.Verify(x => x.GetObjectFromString<It.IsAnyType>(It.IsAny<string>()), Times.Exactly(numberOfEntries));
        }

        private class TestObject { }
    }
}

