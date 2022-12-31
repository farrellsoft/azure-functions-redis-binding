
using Farrellsoft.Azure.Functions.Extensions.Redis.Builders;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Farrellsoft.Azure.Functions.Extensions.Redis
{
    [Extension("Redis")]
    internal class RedisExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var bindingRule = context.AddBindingRule<RedisAttribute>();
            bindingRule
                .BindToInput<List<DocumentOpenType>>(typeof(RedisEnumerableBuilder<>));

            bindingRule
                .BindToInput<DocumentOpenType>(typeof(RedisItemBuilder<>));
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