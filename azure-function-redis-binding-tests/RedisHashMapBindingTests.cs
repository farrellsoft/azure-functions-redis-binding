﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Farrellsoft.Azure.Functions.Extensions.Redis.Bindings;
using Farrellsoft.Azure.Functions.Extensions.Redis.ValueProviders;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Tests;
using Xunit;

namespace azure_function_redis_binding_tests
{
    public class given_an_instance_of_RedisHashMapBinding
    {
        [Fact]
        public async Task validate_the_returned_value_provider_is_RedisHashMapValueProvider_with_the_correct_inner_type()
        {
            // arrange
            var binding = new RedisHashMapBinding<string>(
                MoqHelper.GetRedisAttribute("somekey", "SomeConnection"),
                MoqHelper.BuildConfiguration(new Dictionary<string, string> { { "SomeConnection", "SomeValue" } }));

            var context = new BindingContext(
                valueContext: MoqHelper.GetValueBindingContext(),
                bindingData: new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()));

            // act
            var valueProvider = await binding.BindAsync(context);

            // assert
            Assert.True(valueProvider is RedisHashMapValueProvider<string>);
        }
    }
}