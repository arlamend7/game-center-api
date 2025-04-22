using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SGTC.IOC.Injectors
{
    public static class SettingsInjector
    {
        public static IServiceCollection ResolveOptions<T>(this IServiceCollection services, IConfiguration configuration) where T : class, new()
        {
            services.Configure<T>(configuration);

            // Register main options as a normal singleton
            services.AddSingleton(typeof(T), sp => configuration.Get<T>());

            var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethod("Configure", new[] { typeof(IServiceCollection), typeof(IConfiguration) });

            ConfigureSubOptions(configureMethod, typeof(T), services, configuration);
            return services;
        }

        private static void ConfigureSubOptions(MethodInfo configureMethod, Type type, IServiceCollection services, IConfiguration configuration)
        {
            foreach (var property in type.GetProperties())
            {
                var configurationSection = configuration.GetSection(property.Name);
                var subType = property.PropertyType;

                if (!subType.IsClass || subType == typeof(string))
                    continue;

                // Register sub-options as keyed singletons using the property name
                var obj = Activator.CreateInstance(subType);
                configurationSection.Bind(obj);
                services.AddKeyedSingleton(subType, property.Name, obj);

                // Configure binding (optional if you only use Get<T>())
                var configuredMethod = configureMethod.MakeGenericMethod(subType);
                configuredMethod.Invoke(null, new object[] { services, configurationSection });

                // Recursive registration
                ConfigureSubOptions(configureMethod, subType, services, configurationSection);
            }
        }
    }

}
