using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Farrellsoft.Azure.Functions.Extensions.Redis;
using Farrellsoft.Azure.Functions.Extensions.Redis.Bindings;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Tests
{
	public class given_an_instance_of_RedisItemBinding
    {
		[Fact]
        public async Task validate_a_string_inner_type_returns_RedisStringValueProvider()
        {
            // arrange
            var binding = new RedisItemBinding(
                MoqHelper.GetRedisAttribute("somekey", "SomeConnection"),
                new Mock<IRedisValueConverter>().Object,
                typeof(string));

            var context = new BindingContext(
                valueContext: MoqHelper.GetValueBindingContext(),
                bindingData: new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()));

            // act
            var valueProvider = await binding.BindAsync(context);

            // assert
            Assert.True(valueProvider is RedisStringValueProvider);
        }

        [Fact]
        public async Task validate_a_single_non_string_type_returns_RedisObjectValueProvider()
        {
            // arrange
            var binding = new RedisItemBinding(
                MoqHelper.GetRedisAttribute("somekey", "SomeConnection"),
                new Mock<IRedisValueConverter>().Object,
                typeof(object));

            var context = new BindingContext(
                valueContext: MoqHelper.GetValueBindingContext(),
                bindingData: new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()));

            // act
            var valueProvider = await binding.BindAsync(context);

            // assert
            Assert.True(valueProvider is RedisObjectValueProvider<object>);
        }
    }
}

