using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Moq;
using Xunit;

namespace Tests
{
	public class given_an_instance_of_RedisObjectValueProvider
    {
		[Fact]
		public async void validate_returned_result_iss_intended_object()
		{
            // arrange
            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetObject<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new TestObject() { Value = "test" });

            var provider = new RedisObjectValueProvider<TestObject>("someConnection", "someKey", clientMock.Object);

            // act
            var result = await provider.GetValueAsync();

            // assert
            var typedResult = result as TestObject;
            Assert.NotNull(typedResult);
            Assert.Equal("test", typedResult.Value);
		}

        private class TestObject
        {
            public string Value { get; set; }
        }
    }
}

