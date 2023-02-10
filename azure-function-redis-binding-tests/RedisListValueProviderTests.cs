using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Moq;
using Xunit;

namespace Tests
{
	public class given_an_instance_of_RedisListValueProvider
	{
		[Fact]
		public async void validate_RedisListValueProvider_invokes_the_GetList_method_on_client_within_GetValue()
		{
			// arrange
			var converterMock = new Mock<IRedisValueConverter>();
			converterMock.Setup(x => x.GetList<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new List<TestObject>());

            var provider = new RedisListValueProvider<TestObject>("someConnection", "someKey", converterMock.Object);

            // act
            await provider.GetValueAsync();

			// assert
			converterMock.Verify(x => x.GetList<TestObject>("someConnection", "someKey"), Times.Once);
        }

		[Fact]
		public async void validate_RedisListProvider_returns_an_object_of_type_List_TValue_on_invokation()
		{
            // arrange
            var converterMock = new Mock<IRedisValueConverter>();
            converterMock.Setup(x => x.GetList<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<TestObject>());

            var provider = new RedisListValueProvider<TestObject>("someConnection", "someKey", converterMock.Object);

            // act
            var result = await provider.GetValueAsync();

			// assert
			var typedResult = result as List<TestObject>;
			Assert.NotNull(typedResult);
        }

        private class TestObject { }
	}
}

