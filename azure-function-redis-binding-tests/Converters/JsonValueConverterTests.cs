using System;
using Farrellsoft.Azure.Functions.Extensions.Redis.Converters;
using Xunit;

namespace Tests.Converters
{
	public class given_an_instance_of_JsonValueConverter
	{
		[Fact]
		public void validate_an_empty_string_returns_a_null_value()
		{
			// arrange
			var converter = new JsonValueConverter();

			// act
			var result = converter.GetObjectFromString<TestObject>(string.Empty);

			// assert
			Assert.Null(result);
		}

		[Fact]
		public void validate_a_null_string_returns_null()
		{
            // arrange
            var converter = new JsonValueConverter();

            // act
            var result = converter.GetObjectFromString<TestObject>(null);

            // assert
            Assert.Null(result);
        }

		[Fact]
		public void validate_a_valid_json_string_is_returned_as_an_object_instance_of_the_appropriate_type()
		{
            // arrange
            var converter = new JsonValueConverter();

            // act
            var result = converter.GetObjectFromString<TestObject>("{\"firstName\": \"Test\"}");

			// assert
			Assert.True(result is TestObject);
			Assert.Equal("Test", result.FirstName);
        }

		private class TestObject { public string FirstName { get; set; } }
	}
}

