
using Farrellsoft.Azure.Functions.Extensions.Redis;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(RedisWebJobsStartup))]
namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
    public class RedisWebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddRedis();
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