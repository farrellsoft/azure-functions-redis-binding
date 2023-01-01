
## Usage
Redis binding currently only supports reading data by key out of the Redis cache. You must provide a **key** and a value for the connection string, represented through the property **Connection**


### Example: Read a simple string
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public static async Task<IActionResult> Run(
  [Redis(key: "value", Connection = "RedisConnectionString")] string value)
{
}
```

### Example: Read a list of strings
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public static async Task<IActionResult> Run(
  [Redis(key: "values", Connection = "RedisConnectionString")] List<string> values)
{
}
```

### Example: Read an object in JSON
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public static async Task<IActionResult> Run(
  [Redis(key: "object", Connection = "RedisConnectionString")] Person person)
{
}
```
**The underlying value should be a JSON string stored as a Redis string**

### Example: Read a list of objects in JSON
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public static async Task<IActionResult> Run(
  [Redis(key: "objects", Connection = "RedisConnectionString")] List<Person> people)
{
}
```
**The underlying value should be JSON strings stored as elements within a Redis list**