using Farrellsoft.Azure.Functions.Extensions.Redis;

namespace Sandbox
{
    public class Person : IRedisListItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; set; }
    }
}