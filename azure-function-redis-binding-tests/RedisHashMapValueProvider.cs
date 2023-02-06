using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Moq;
using Xunit;

namespace Tests
{
	public class given_an_instance_of_RedisHashMapValueProvider
	{
		[Fact]
		public async void validate_the_returned_result_is_a_dictionary_whose_value_type_is_TValue()
		{
			// arrange
			var clientMock = new Mock<IClient>();
			clientMock.Setup(x => x.GetHashMap<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new System.Collections.Generic.Dictionary<string, TestObject>());

			var provider = new RedisHashMapValueProvider<TestObject>("someConnection", "someKey", clientMock.Object);

			// act
			var result = await provider.GetValueAsync();

			// assert
			var typedResult = result as Dictionary<string, TestObject>;
			Assert.NotNull(typedResult);
		}

		[Fact]
		public async void validate_the_instance_calls_the_GetHashMap_method_on_the_client_one_time()
		{
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetHashMap<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new System.Collections.Generic.Dictionary<string, TestObject>());

            var provider = new RedisHashMapValueProvider<TestObject>("someConnection", "someKey", clientMock.Object);

            // act
            await provider.GetValueAsync();

			// assert
			clientMock.Verify(x => x.GetHashMap<TestObject>("someConnection", "someKey"), Times.Once);
        }

        private class TestObject { }
	}
}

