using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Farrellsoft.Azure.Functions.Extensions.Redis;
using Farrellsoft.Azure.Functions.Extensions.Redis.Bindings;
using Farrellsoft.Azure.Functions.Extensions.Redis.Helpers;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Tests;

public class given_an_instance_of_RedisBindingProvider
{
    [Fact]
    public async Task validate_a_non_generic_parameter_returns_RedisItemBinding()
    {
        // arrange
        var configurationMock = new Mock<IConfiguration>();
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[0]);

        var provider = new RedisBindingProvider(configurationMock.Object, paramInfoHelperMock.Object);

        var paramInfoMock = new Mock<ParameterInfo>();
        var context = new BindingProviderContext(
            parameter: paramInfoMock.Object,
            bindingDataContract: new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>()),
            cancellationToken: new System.Threading.CancellationToken());

        // act
        var result = await provider.TryCreateAsync(context);

        // assert
        Assert.True(result is RedisItemBinding);
    }

    [Fact]
    public async Task validate_a_single_generic_type_returns_RedisListBinding()
    {
        var configurationMock = new Mock<IConfiguration>();
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[] { typeof(string) });

        var provider = new RedisBindingProvider(configurationMock.Object, paramInfoHelperMock.Object);

        var paramInfoMock = new Mock<ParameterInfo>();
        var context = new BindingProviderContext(
            parameter: paramInfoMock.Object,
            bindingDataContract: new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>()),
            cancellationToken: new System.Threading.CancellationToken());

        // act
        var result = await provider.TryCreateAsync(context);

        // assert
        Assert.True(result is RedisListBinding<string>);
    }

    [Fact]
    public async Task validate_a_pair_of_generic_types_returns_RedisHashMapBinding()
    {
        var configurationMock = new Mock<IConfiguration>();
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[] { typeof(string), typeof(string) });

        var provider = new RedisBindingProvider(configurationMock.Object, paramInfoHelperMock.Object);

        var paramInfoMock = new Mock<ParameterInfo>();
        var context = new BindingProviderContext(
            parameter: paramInfoMock.Object,
            bindingDataContract: new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>()),
            cancellationToken: new System.Threading.CancellationToken());

        // act
        var result = await provider.TryCreateAsync(context);

        // assert
        Assert.True(result is RedisHashMapBinding<string>);
    }

    [Fact]
    public async Task validate_a_generic_with_more_than_two_types_throws_NotSupportedException()
    {
        var configurationMock = new Mock<IConfiguration>();
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[] { typeof(string), typeof(string), typeof(string) });

        var provider = new RedisBindingProvider(configurationMock.Object, paramInfoHelperMock.Object);

        var paramInfoMock = new Mock<ParameterInfo>();
        var context = new BindingProviderContext(
            parameter: paramInfoMock.Object,
            bindingDataContract: new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>()),
            cancellationToken: new System.Threading.CancellationToken());

        // act
        // assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await provider.TryCreateAsync(context));
    }
}
