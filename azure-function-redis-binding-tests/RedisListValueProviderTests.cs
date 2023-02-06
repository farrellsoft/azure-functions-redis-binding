using System;
using System.Collections.Generic;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
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
			var clientMock = new Mock<IClient>();
			clientMock.Setup(x => x.GetList<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new List<TestObject>());

            var provider = new RedisListValueProvider<TestObject>("someConnection", "someKey", clientMock.Object);

            // act
            await provider.GetValueAsync();

			// assert
			clientMock.Verify(x => x.GetList<TestObject>("someConnection", "someKey"), Times.Once);
        }

		[Fact]
		public async void validate_RedisListProvider_returns_an_object_of_type_List_TValue_on_invokation()
		{
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetList<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<TestObject>());

            var provider = new RedisListValueProvider<TestObject>("someConnection", "someKey", clientMock.Object);

            // act
            var result = await provider.GetValueAsync();

			// assert
			var typedResult = result as List<TestObject>;
			Assert.NotNull(typedResult);
        }

        private class TestObject { }
	}
}

