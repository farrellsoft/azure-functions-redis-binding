using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Farrellsoft.Azure.Functions.Extensions.Redis.Bindings;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Executors;
using Moq;
using Xunit;

namespace Tests.Bindings
{
	public class given_an_instance_of_RedisListBinding
    {
		[Fact]
		public async Task validate_the_returned_value_provider_is_RedisListValueProvider_with_the_correct_inner_type()
		{
            // arrange
            var binding = new RedisListBinding<string>(
                MoqHelper.GetRedisAttribute("somekey", "SomeConnection"),
                new Mock<IRedisValueConverter>().Object);

            var context = new BindingContext(
                valueContext: MoqHelper.GetValueBindingContext(),
                bindingData: new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()));

            // act
            var valueProvider = await binding.BindAsync(context);

            // assert
            Assert.True(valueProvider is RedisListValueProvider<string>);
        }

        [Fact]
        public async Task validate_BindAsync_throws_NotImplementedException()
        {
            // arrange
            var binding = new RedisListBinding<string>(
                MoqHelper.GetRedisAttribute("somekey", "SomeConnection"),
                new Mock<IRedisValueConverter>().Object);

            // act
            // assert
            await Assert.ThrowsAsync<NotImplementedException>(async () => await binding.BindAsync(
                new object(),
                new ValueBindingContext(
                    new FunctionBindingContext(
                        new Mock<IFunctionInstanceEx>().Object, new System.Threading.CancellationToken()),
                    new System.Threading.CancellationToken())));

        }

        [Fact]
        public void validate_ToParameterDescriptor_returns_a_non_null_object()
        {
            // arrange
            var binding = new RedisListBinding<string>(
                MoqHelper.GetRedisAttribute("somekey", "SomeConnection"),
                new Mock<IRedisValueConverter>().Object);

            // act
            var result = binding.ToParameterDescriptor();

            // assert
            Assert.NotNull(result);
        }
    }
}

