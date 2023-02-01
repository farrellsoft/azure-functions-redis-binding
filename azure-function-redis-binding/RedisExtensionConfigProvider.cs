
using Farrellsoft.Azure.Functions.Extensions.Redis.Builders;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;

namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
    [Extension("Redis")]
    internal class RedisExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly IConfiguration _configuration;

        public RedisExtensionConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            var bindingRule = context.AddBindingRule<RedisAttribute>();
            bindingRule.AddValidator(ValidateAttribute);

            bindingRule
                .BindToCollector<DocumentOpenType>(typeof(RedisCollectorBuilder<>));

            bindingRule.Bind(new RedisBindingProvider(_configuration, new Helpers.ParameterInfoHelper()));
        }

        private void ValidateAttribute(RedisAttribute attribute, Type type)
        {
            if (string.IsNullOrEmpty(attribute.Connection))
                throw new InvalidOperationException($"{nameof(RedisAttribute.Connection)} cannot be empty");

            if (string.IsNullOrEmpty(attribute.Key))
                throw new InvalidOperationException($"{nameof(RedisAttribute.Key)} cannot be empty");
        }

        private class DocumentOpenType : OpenType.Poco
        {
            public override bool IsMatch(System.Type type, OpenTypeMatchContext context)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return false;

                if (type.FullName == "System.Object")
                    return true;

                return base.IsMatch(type, context);
            }
        }
    }
}