using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Tests.Converters
{
	public class given_an_instance_of_RedisValueConverter_calling_GetList
    {
        [Fact]
        public async void validate_the_correct_number_of_values_are_returned_given_a_set_of_RedisValue()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetValues(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new StackExchange.Redis.RedisValue[]
                {
                    new StackExchange.Redis.RedisValue("test"),
                    new StackExchange.Redis.RedisValue("user")
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
            var result = await converter.GetList<string>("validConnectionName", "someKey");

            // assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void validate_for_a_inner_type_of_string_no_call_to_GetObjectFromString_is_made()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetValues(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new StackExchange.Redis.RedisValue[]
                {
                    new StackExchange.Redis.RedisValue("test")
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
            var result = await converter.GetList<string>("validConnectionName", "someKey");

            // assert
            converterMock.Verify(x => x.GetObjectFromString<It.IsAnyType>(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void validate_for_a_inner_type_is_a_class_the_right_number_of_calls_to_GetObjectFromString_are_made(int numberOfEntries)
        {
            // arrange
            var entries = new StackExchange.Redis.RedisValue[numberOfEntries];
            for (int i = 0; i < numberOfEntries; i++)
            {
                entries[i] = new StackExchange.Redis.RedisValue("{}");
            }

            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetValues(It.IsAny<string>(), It.IsAny<string>()))
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
            var result = await converter.GetList<TestObject>("validConnectionName", "someKey");

            // assert
            converterMock.Verify(x => x.GetObjectFromString<It.IsAnyType>(It.IsAny<string>()), Times.Exactly(numberOfEntries));
        }

        private class TestObject { }
    }
}

