using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Farrellsoft.Azure.Functions.Extensions.Redis;

namespace Sandbox
{
    public class HttpTestBinding
    {
        [FunctionName("TestSandbox")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test")] HttpRequest req,
            [Redis(key: "list1", valueType: RedisValueType.Collection, Connection = "RedisConnectionString")] ICollector<Person> values,
            ILogger log)
        {
            values.Add(new Person { Id = "1", Name = "Jason2" });
            values.Add(new Person { Id = "2", Name = "Claire3" });
            values.Add(new Person { Id = "3", Name = "Ethan" });

            //return new OkObjectResult(String.Join(",", values));
            return new OkObjectResult("done");
        }
    }
}
