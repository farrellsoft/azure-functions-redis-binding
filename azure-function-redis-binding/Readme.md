## Azure Function Redis CSharp Binding
Bind your functions to data in Redis in either a read mode or a write mode, for single values or list of values.

#### Requirements
Built on .NET 6 (LTS) and leveraging Newtonsoft Json.NET internally.
Include the package **Farrellsoft.Azure.Functions.Extensions.Redis**

#### Limitations
Right now the attribute is limited to working with the Redis string value and Redis list values types. Hash support is planned for the future.

Specifying Time To Live for values is not currently supported, this will be supported in the near future.

## Reading Values

#### Single Value Read
Use the **Redis** attribute targeting a specific key to read the value out of your Redis instance.

##### Code Example: Read a string
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public static async Task<IActionResult> Run(
  [Redis(key: "value", Connection = "RedisConnectionString")] string value)
{
}
```

##### Code Example: Read a object
You can read typed objects from Redis IF the underlying data is stored in JSON. It will be deserialized using Json.NET
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public static async Task<IActionResult> Run(
  [Redis(key: "object", Connection = "RedisConnectionString")] Person person)
{
}
```

#### List Value Read
Redis lists are supported for either strings or objects represented by JSON strings

##### Code Example: Read a list of strings
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public static async Task<IActionResult> Run(
  [Redis(key: "values", Connection = "RedisConnectionString")] List<string> values)
{
}
```

##### Code Example: Read a list of objects
As was the case with reading an single object out of Redis, individual items need to be JSON strings which can be deserialized by Json.NET
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public static async Task<IActionResult> Run(
  [Redis(key: "objects", Connection = "RedisConnectionString")] List<Person> people)
{
}
```

## Writing Values
The same attribute can be used to write information back into Redis. In this scenario, a value is required to inform the attribute what the underlying value type in Redis is.

#### Value Type
The supported values for **ValueType** are:
- Single => will leverage the string related APIs for Redis
- Collection => will leverage the List related APIs for Redis

All example will show using **ICollector&lt;T&gt;**, however, you may also use **IAsyncCollector&lt;T&gt;**

##### Code Example: Write a string value to the cache
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public async Task<IActionResult> Run(
  [Redis(key: "value", valueType: RedisValueType.Single, Connection = "RedisConnectionString")] ICollector<string> values)
{
  values.Add("testvalue")
}
```
This will overwrite the existing value which is present

##### Code Example: Write an object to the cache
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public async Task<IActionResult> Run(
  [Redis(key: "value", valueType: RedisValueType.Single, Connection = "RedisConnectionString")] ICollector<Person> values)
{
  values.Add(new Person { Name = "Foo" });
}
```
**Note:** the Person instance shown above will be stored as a string in Redis serialized using Json.NET

##### Code Example: Write a string to a list of values
The attribute can be used, in conjunction with **ValueType.Collection** to append values to a Redis list
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public async Task<IActionResult> Run(
  [Redis(key: "values", valueType: RedisValueType.Collection, Connection = "RedisConnectionString")] ICollector<string> values)
{
  values.Add("test1");
  values.Add("test2");
}
```

##### Code Example: Write an object to a list of values
Object values are written as JSON using Serialization through the Json.NET library
```
using Farrellsoft.Azure.Functions.Extensions.Redis;
...
public async Task<IActionResult> Run(
  [Redis(key: "values", valueType: RedisValueType.Collection, Connection = "RedisConnectionString")] ICollector<Person> values)
{
  values.Add(new Person { Name = "Name 1" });
  values.Add(new Person { Name = "Name 2" });
}
```

##### Code Example: Update an existing object in a Redis list
The binding offers the ability to update an existing item in a Redis list via a user generated ID value. To activate this functionality, the POCO being stored must implement the **IRedisListItem** interface - note in place updating is NOT available for non-objects
```
namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
	public interface IRedisListItem
	{
		public string Id { get; set; }
	}
}
```

This **Id** property must be hydrated with a value that can be used to look up the value in the underlying collection.

```
namespace Sandbox
{
    public class Person : IRedisListItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
```

Once specified, the binding will fetch all list values for the list and search for the correct item. When found it will update, if it is not found, it will fall back to appending the item, again in JSON
```
public async Task<IActionResult> Run(
  [Redis(key: "values", valueType: RedisValueType.Collection, Connection = "RedisConnectionString")] ICollector<Person> values)
{
  values.Add(new Person { Id = "1", Name = "Name 1" });
  values.Add(new Person { Id = "1", Name = "Name 2" });
}
```

#### Code Example: Retrieve a HashMap from Redis based on a key (v1.2+)
This can be done with either <strong>Dictionary&lt;string, TValue&gt; or Dictionary&lt;string, string&gt;
```
public async Task<IActionResult> Run(
  [RedisHash(key: "brothers", Connection = "RedisConnectionString")] Dictionary<string, Person> values)
{
  var valueString = string.Join(",", values.Values);

  return new OkObjectResult(valueString);
}
```

## Support
Feel free to contact me on Twitter (@jfarrell) or Mastodon (https://hachyderm.io/@jfarrell) with any questions or feature requests. As time goes on, I intend to create more process around asks.