using Farrellsoft.Azure.Functions.Extensions.Redis;

namespace Sandbox
{
    public class Person : IRedisListItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}