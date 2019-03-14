using System;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace DepressedBot.Discord
{
    public sealed class DepressedBot
    {
        public static readonly IServiceProvider ServiceProvider = BuildProvider();

        public static readonly CommandService CommandService = GetService<CommandService>();

        public static readonly DiscordSocketClient Client = GetService<DiscordSocketClient>();

        public static T GetService<T>() => ServiceProvider.GetRequiredService<T>();

        private static IServiceProvider BuildProvider()
        {
            return new ServiceCollection()
                .AddSingleton(new CommandService(new CommandServiceConfiguration
                {
                    IgnoreExtraArguments = true,
                    CaseSensitive = false,
                    DefaultRunMode = RunMode.Sequential,
                    SeparatorRequirement = SeparatorRequirement.Separator,
                    Separator = "irrelevant",
                    NullableNouns = null
                }))
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    AlwaysDownloadUsers = true,
                    ConnectionTimeout = 10000,
                    MessageCacheSize = 50
                }))
                .BuildServiceProvider();
        }
    }
}