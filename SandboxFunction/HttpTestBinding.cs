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
            [Redis(key: "values", valueType: RedisValueType.Collection, Connection = "RedisConnectionString")] ICollector<Person> values)
        {

            values.Add(new Person { Name = "Name 1" });



            return null;
        }
    }
}
