using System;
namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
	internal interface IRedisAttribute
	{
		public string? Connection { get; }
		public string Key { get; }
	}
}

