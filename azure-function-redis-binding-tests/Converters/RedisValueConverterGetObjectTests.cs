using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Tests.Converters
{
	public class given_an_instance_of_RedisValueConverter_calling_GetObject
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => converter.GetObject<TestObject>(string.Empty, "someKey"));
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
            await Assert.ThrowsAsync<ArgumentException>(() => converter.GetObject<TestObject>("connectionName", "someKey"));
        }

        [Fact]
        public async void validate_the_type_returned_by_GetObject_returns_an_object_of_the_requested_type()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetString(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("{}");
            var configuration = MoqHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "validConnectionName", "someConnectionString" }
            });

            var converterMock = new Mock<IValueConverter>();
            converterMock.Setup(x => x.GetObjectFromString<TestObject>(It.IsAny<string>()))
                .Returns(new TestObject { FirstName = "tester" });

            var converter = new RedisValueConverter(
                clientMock.Object,
                configuration,
                converterMock.Object);

            // act
            var result = await converter.GetObject<TestObject>("validConnectionName", "someKey");

            // assert
            Assert.Equal("tester", result.FirstName);
        }

        [Fact]
        public async void validate_null_is_returned_if_the_string_returned_by_client_is_empty()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetString(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(string.Empty);
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
            var result = await converter.GetObject<TestObject>("validConnectionName", "someKey");

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async void validate_for_a_return_type_of_object_one_call_to_GetObjectFromString_is_made()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetString(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("{}");
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
            var result = await converter.GetObject<TestObject>("validConnectionName", "someKey");

            // assert
            converterMock.Verify(x => x.GetObjectFromString<TestObject>(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void validate_one_call_to_GetString_on_client_is_made()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetString(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(string.Empty);
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
            await converter.GetObject<TestObject>("validConnectionName", "someKey");

            // assert
            clientMock.Verify(x => x.GetString("someConnectionString", "someKey"), Times.Once);
        }

        private class TestObject { public string FirstName { get; set; } }
    }
}

