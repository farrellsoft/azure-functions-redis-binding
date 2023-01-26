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

namespace Sandbox
{
    public class HttpTestBinding
    {
        [FunctionName("TestSandbox")]
        public async Task<IActionResult> Run(
            [Redis(key: "brothers", Connection = "RedisConnectionString")] Dictionary<string, string> values)
        {

            var valueString = string.Join(",", values.Values);

            return new OkObjectResult(valueString);
        }
    }
}
