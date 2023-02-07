using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Farrellsoft.Azure.Functions.Extensions.Redis;
using Farrellsoft.Azure.Functions.Extensions.Redis.Bindings;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
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
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[0]);
        paramInfoHelperMock.Setup(x => x.GetParameterType(It.IsAny<ParameterInfo>()))
            .Returns(typeof(string));

        var provider = new RedisBindingProvider(new Mock<IClient>().Object, paramInfoHelperMock.Object);

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
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[] { typeof(string) });
        paramInfoHelperMock.Setup(x => x.GetParameterType(It.IsAny<ParameterInfo>()))
            .Returns(typeof(List<string>));

        var provider = new RedisBindingProvider(new Mock<IClient>().Object, paramInfoHelperMock.Object);

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
        // arrange
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[] { typeof(string), typeof(string) });
        paramInfoHelperMock.Setup(x => x.GetParameterType(It.IsAny<ParameterInfo>()))
            .Returns(typeof(Dictionary<string, string>));

        var provider = new RedisBindingProvider(new Mock<IClient>().Object, paramInfoHelperMock.Object);

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
        // arrange
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[] { typeof(string), typeof(string), typeof(string) });

        var provider = new RedisBindingProvider(new Mock<IClient>().Object, paramInfoHelperMock.Object);

        var paramInfoMock = new Mock<ParameterInfo>();
        var context = new BindingProviderContext(
            parameter: paramInfoMock.Object,
            bindingDataContract: new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>()),
            cancellationToken: new System.Threading.CancellationToken());

        // act
        // assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await provider.TryCreateAsync(context));
    }

    [Fact]
    public async void validate_if_a_non_string_or_object_is_bound_to_a_NotSupportedException_is_raised()
    {
        // arrange
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[0]);
        paramInfoHelperMock.Setup(x => x.GetParameterType(It.IsAny<ParameterInfo>()))
            .Returns(typeof(int));

        var provider = new RedisBindingProvider(new Mock<IClient>().Object, paramInfoHelperMock.Object);

        var paramInfoMock = new Mock<ParameterInfo>();
        var context = new BindingProviderContext(
            parameter: paramInfoMock.Object,
            bindingDataContract: new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>()),
            cancellationToken: new System.Threading.CancellationToken());

        // act
        // assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await provider.TryCreateAsync(context));
    }

    [Fact]
    public async void validate_if_a_non_List_type_is_bound_a_NotSupportedException_is_raised()
    {
        // arrange
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[1]);
        paramInfoHelperMock.Setup(x => x.GetParameterType(It.IsAny<ParameterInfo>()))
            .Returns(typeof(Collection<string>));

        var provider = new RedisBindingProvider(new Mock<IClient>().Object, paramInfoHelperMock.Object);

        var paramInfoMock = new Mock<ParameterInfo>();
        var context = new BindingProviderContext(
            parameter: paramInfoMock.Object,
            bindingDataContract: new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>()),
            cancellationToken: new System.Threading.CancellationToken());

        // act
        // assert
        await Assert.ThrowsAsync<NotSupportedException>(async () => await provider.TryCreateAsync(context));
    }

    [Fact]
    public async void validate_if_a_non_Dictionary_type_is_bound_a_NotSupportedException_is_raised()
    {
        // arrange
        var paramInfoHelperMock = new Mock<IParameterInfoHelper>();
        paramInfoHelperMock.Setup(x => x.GetGenericTypeArgs(It.IsAny<ParameterInfo>()))
            .Returns(new Type[2]);
        paramInfoHelperMock.Setup(x => x.GetParameterType(It.IsAny<ParameterInfo>()))
            .Returns(typeof(Tuple<string, string>));

        var provider = new RedisBindingProvider(new Mock<IClient>().Object, paramInfoHelperMock.Object);

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
