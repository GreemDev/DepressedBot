using System;
using System.Linq;
using System.Reflection;
using DepressedBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DepressedBot.Extensions
{
    public static class DependencyInjectionExtensions
    {

        public static IServiceCollection AddDepressedBotServices(this ServiceCollection provider)
        {
            foreach (var service in Assembly.GetEntryAssembly().GetTypes()
                .Where(t => !t.HasAttribute<ObsoleteAttribute>() &&
                            t.HasAttribute<ServiceAttribute>()))
            {
                provider.AddSingleton(service);
            }
            return provider;
        }

    }
}
