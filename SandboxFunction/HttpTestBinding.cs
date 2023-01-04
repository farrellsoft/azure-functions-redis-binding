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
            [Redis(key: "list2", valueType: RedisValueType.Collection, Connection = "RedisConnectionString")] ICollector<Person> values,
            ILogger log)
        {
            values.Add(new Person { Name = "Bob" });
            values.Add(new Person { Name = "Claire" });

            //return new OkObjectResult(String.Join(",", values));
            return new OkObjectResult("done");
        }
    }
}
