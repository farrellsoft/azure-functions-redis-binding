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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandbox
{
    public class HttpTestBinding
    {
        [FunctionName("TestSandbox")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test")] HttpRequest request,
            [Redis("brothers", Connection = "RedisConnectionString")] Dictionary<string, Person> values)
        {   
            var strings = values.Select(x => $"age: {x.Key} name: {x.Value.FirstName}");
            return new OkObjectResult(String.Join(",", strings));
        }
    }
}
