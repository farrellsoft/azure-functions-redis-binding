using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Tests.Converters
{
	public class given_an_instance_of_RedisValueConverter_calling_IClient
	{
		[Fact]
		public async void validate_if_no_connectionName_is_passed_when_calling_GetDictionary_ArgumentNullException_is_raised()
		{
			// arrange
			var converter = new RedisValueConverter(
				new Mock<IClient>().Object,
				new Mock<IConfiguration>().Object,
                new Mock<IValueConverter>().Object);

			// act
			// assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await converter.GetDictionary<string>(string.Empty, "someKey"));
		}

		[Fact]
		public async void validate_if_provided_connectionName_to_GetDictionary_does_not_have_a_value_in_configuration_ArgumentException_is_raised()
		{
			// arrange
			var converter = new RedisValueConverter(
				new Mock<IClient>().Object,
				MoqHelper.BuildConfiguration(new Dictionary<string, string>()),
                new Mock<IValueConverter>().Object);

			// act
			// assert
			await Assert.ThrowsAsync<ArgumentException>(async () => await converter.GetDictionary<string>("someValue", "someKey"));
		}

        [Fact]
        public async void validate_a_call_to_GetDictionary_calls_GetHashEntries()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            var configruation = MoqHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "someConnection", "connectionString" }
            });
            var converter = new RedisValueConverter(clientMock.Object, configruation, new Mock<IValueConverter>().Object);

            // act
            await converter.GetDictionary<string>("someConnection", "someKey");

            // assert
            clientMock.Verify(x => x.GetHashEntries(It.IsAny<string>(), "someKey"), Times.Once);
        }

        [Fact]
        public async void validate_if_no_connectionName_is_passed_when_calling_GetList_ArgumentNullException_is_raised()
        {
            // arrange
            var converter = new RedisValueConverter(
                new Mock<IClient>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IValueConverter>().Object);

            // act
            // assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await converter.GetList<string>(string.Empty, "someKey"));
        }

        [Fact]
        public async void validate_if_provided_connectionName_to_GetList_does_not_have_a_value_in_configuration_ArgumentException_is_raised()
        {
            // arrange
            var converter = new RedisValueConverter(
                new Mock<IClient>().Object,
                MoqHelper.BuildConfiguration(new Dictionary<string, string>()),
                new Mock<IValueConverter>().Object);

            // act
            // assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await converter.GetList<string>("someValue", "someKey"));
        }

        [Fact]
        public async void validate_a_call_to_GetList_calls_GetValues()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            var configruation = MoqHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "someConnection", "connectionString" }
            });
            var converter = new RedisValueConverter(clientMock.Object, configruation, new Mock<IValueConverter>().Object);

            // act
            await converter.GetList<string>("someConnection", "someKey");

            // assert
            clientMock.Verify(x => x.GetValues(It.IsAny<string>(), "someKey"), Times.Once);
        }

        [Fact]
        public async void validate_if_no_connectionName_is_passed_when_calling_GetObject_ArgumentNullException_is_raised()
        {
            // arrange
            var converter = new RedisValueConverter(
                new Mock<IClient>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IValueConverter>().Object);

            // act
            // assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await converter.GetObject<TestObject>(string.Empty, "someKey"));
        }

        [Fact]
        public async void validate_if_provided_connectionName_to_GetObject_does_not_have_a_value_in_configuration_ArgumentException_is_raised()
        {
            // arrange
            var converter = new RedisValueConverter(
                new Mock<IClient>().Object,
                MoqHelper.BuildConfiguration(new Dictionary<string, string>()),
                new Mock<IValueConverter>().Object);

            // act
            // assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await converter.GetObject<TestObject>("someValue", "someKey"));
        }

        [Fact]
        public async void validate_a_call_to_GetObject_calls_GetString()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            var configruation = MoqHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "someConnection", "connectionString" }
            });
            var converter = new RedisValueConverter(clientMock.Object, configruation, new Mock<IValueConverter>().Object);

            // act
            await converter.GetObject<TestObject>("someConnection", "someKey");

            // assert
            clientMock.Verify(x => x.GetString(It.IsAny<string>(), "someKey"), Times.Once);
        }

        private class TestObject { }

        [Fact]
        public async void validate_if_no_connectionName_is_passed_when_calling_GetString_ArgumentNullException_is_raised()
        {
            // arrange
            var converter = new RedisValueConverter(
                new Mock<IClient>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IValueConverter>().Object);

            // act
            // assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await converter.GetString(string.Empty, "someKey"));
        }

        [Fact]
        public async void validate_if_provided_connectionName_to_GetString_does_not_have_a_value_in_configuration_ArgumentException_is_raised()
        {
            // arrange
            var converter = new RedisValueConverter(
                new Mock<IClient>().Object,
                MoqHelper.BuildConfiguration(new Dictionary<string, string>()),
                new Mock<IValueConverter>().Object);

            // act
            // assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await converter.GetString("someValue", "someKey"));
        }

        [Fact]
        public async void validate_a_call_to_GetString_calls_GetString()
        {
            // arrange
            var clientMock = new Mock<IClient>();
            var configruation = MoqHelper.BuildConfiguration(new Dictionary<string, string>
            {
                { "someConnection", "connectionString" }
            });
            var converter = new RedisValueConverter(clientMock.Object, configruation, new Mock<IValueConverter>().Object);

            // act
            await converter.GetString("someConnection", "someKey");

            // assert
            clientMock.Verify(x => x.GetString(It.IsAny<string>(), "someKey"), Times.Once);
        }
    }
}

