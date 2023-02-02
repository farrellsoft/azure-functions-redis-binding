using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Farrellsoft.Azure.Functions.Extensions.Redis;
using Farrellsoft.Azure.Functions.Extensions.Redis.Bindings;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace azure_function_redis_binding_tests
{
	public class given_an_instance_of_RedisItemBinding
    {
		[Fact]
        public async Task validate_a_string_inner_type_returns_RedisStringValueProvider()
        {
            // arrange
            var binding = new RedisItemBinding(
                new RedisAttribute(string.Empty) {  Connection = "SomeConnection" },
                MoqHelper.BuildConfiguration(new Dictionary<string, string> { { "SomeConnection", "SomeValue" } }),
                typeof(string));

            var valueBindingContext = new ValueBindingContext(
                new FunctionBindingContext(
                    functionInstance: new Mock<IFunctionInstanceEx>().Object,
                    functionCancellationToken: new System.Threading.CancellationToken()),
                cancellationToken: new System.Threading.CancellationToken());

            var context = new BindingContext(
                valueContext: valueBindingContext,
                bindingData: new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()));

            // act
            var valueProvider = await binding.BindAsync(context);

            // assert
            Assert.True(valueProvider is RedisStringValueProvider);
        }
    }
}

