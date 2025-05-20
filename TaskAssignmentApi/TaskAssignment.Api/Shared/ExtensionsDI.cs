using System.Reflection;

namespace TaskAssignment.Api.Shared
{
    public static class ExtensionsDI
    {
        public static void AddScopedAllByTypeRule(this IServiceCollection services, Assembly targetAssembly, Func<Type, bool> typeRule, Func<Type, bool>? interfaceTypeRule = null)
        {
            interfaceTypeRule ??= new Func<Type, bool>(i => true);

            targetAssembly
                .GetTypes()
                .Where(typeRule)
                .Select(t => new { implementType = t, interfaceType = t.GetInterfaces().First(interfaceTypeRule) })
                .ToList()
                .ForEach(item =>
                {
                    services.AddScoped(item.interfaceType, item.implementType);
                });
        }

        public static void AddScopedServices(this IServiceCollection services, Assembly targetAssembly)
        {
            services.AddScopedAllByTypeRule(targetAssembly,
                t => t.Name.EndsWith("Service") && !t.IsAbstract && !t.IsInterface,
                i => i.Name.EndsWith("Service"));
        }

        public static void AddScopedRepositories(this IServiceCollection services, Assembly targetAssembly)
        {
            services.AddScopedAllByTypeRule(targetAssembly,
                t => t.Name.EndsWith("Repository") && !t.IsAbstract && !t.IsInterface,
                i => i.Name.EndsWith("Repository"));
        }
    }
}
