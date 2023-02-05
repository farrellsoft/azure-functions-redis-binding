
using Farrellsoft.Azure.Functions.Extensions.Redis;
using Farrellsoft.Azure.Functions.Extensions.Redis.Clients;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(RedisWebJobsStartup))]
namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
    public class RedisWebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddRedis();
            builder.Services.AddTransient<IClient, RedisClient>();
        }
    }

    public static class RedisWebJobsExtensions
    {
        public static IWebJobsBuilder AddRedis(this IWebJobsBuilder builder)
        {
            builder.AddExtension<RedisExtensionConfigProvider>();

            return builder;
        }
    }
}