using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Moq;
using Xunit;

namespace Tests.ValueProviders
{
	public class given_an_instance_of_RedisHashMapValueProvider
	{
		[Fact]
		public async void validate_the_returned_result_is_a_dictionary_whose_value_type_is_TValue()
		{
			// arrange
			var converterMock = new Mock<IRedisValueConverter>();
			converterMock.Setup(x => x.GetDictionary<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new System.Collections.Generic.Dictionary<string, TestObject>());

			var provider = new RedisHashMapValueProvider<TestObject>("someConnection", "someKey", converterMock.Object);

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
            var converterMock = new Mock<IRedisValueConverter>();
            converterMock.Setup(x => x.GetDictionary<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new System.Collections.Generic.Dictionary<string, TestObject>());

            var provider = new RedisHashMapValueProvider<TestObject>("someConnection", "someKey", converterMock.Object);

            // act
            await provider.GetValueAsync();

			// assert
			converterMock.Verify(x => x.GetDictionary<TestObject>("someConnection", "someKey"), Times.Once);
        }

		[Fact]
		public void validate_the_ValueProvider_Type_property_returns_the_correct_type()
		{
            // arrange
            var converterMock = new Mock<IRedisValueConverter>();
            var provider = new RedisHashMapValueProvider<TestObject>("someConnection", "someKey", converterMock.Object);

			// act
			var type = provider.Type;

			// assert
			Assert.Equal(typeof(Dictionary<string, TestObject>), type);
        }

        private class TestObject { }
	}
}

