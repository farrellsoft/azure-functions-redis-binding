using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Moq;
using Xunit;

namespace Tests.ValueProviders
{
    public class given_an_instance_of_RedisObjectValueProvider
    {
		[Fact]
		public async void validate_RedisObjectValueProvider_returns_an_object_matching_the_target_type()
		{
            // arrange
            var converterMock = new Mock<IRedisValueConverter>();
            converterMock.Setup(x => x.GetObject<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new TestObject());

            var provider = new RedisObjectValueProvider<TestObject>("someConnection", "someKey", converterMock.Object);

            // act
            var result = await provider.GetValueAsync();

            // assert
            var typedResult = result as TestObject;
            Assert.NotNull(typedResult);
		}

        [Fact]
        public async void validate_RedisObjectValueProvider_invokes_the_GetObject_method()
        {
            // arrange
            var converterMock = new Mock<IRedisValueConverter>();
            converterMock.Setup(x => x.GetObject<TestObject>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new TestObject());

            var provider = new RedisObjectValueProvider<TestObject>("someConnection", "someKey", converterMock.Object);

            // act
            await provider.GetValueAsync();

            // assert
            converterMock.Verify(x => x.GetObject<TestObject>("someConnection", "someKey"), Times.Once);
        }

        private class TestObject
        {
        }
    }
}

