using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DepressedBot.Data;
using DepressedBot.Data.Objects;
using DepressedBot.Extensions;
using DepressedBot.Services;
using Discord;
using Discord.Net.Queue;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace DepressedBot.Discord
{
    public sealed class DepressedBot
    {
        public static readonly ServiceProvider ServiceProvider = BuildProvider();
        public static readonly CommandService CommandService = GetService<CommandService>();
        public static readonly DiscordSocketClient Client = GetService<DiscordSocketClient>();
        public static readonly CancellationTokenSource Cts = new CancellationTokenSource();

        public static T GetService<T>() => ServiceProvider.GetRequiredService<T>();

        private readonly DepressedHandler _handler = GetService<DepressedHandler>();
        private readonly LoggingService _logger = GetService<LoggingService>();

        private DepressedBot()
        { }

        private static ServiceProvider BuildProvider()
        {
            return new ServiceCollection()
                .AddDepressedBotServices()
                .AddSingleton<DepressedHandler>()
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

        public static Task StartAsync() 
            => new DepressedBot().LoginAsync();

        private async Task LoginAsync()
        {
            Console.Title = "DepressedBot";
            Console.CursorVisible = false;
            await AutoResponses.CreateIfNotExists();
            if (!Directory.Exists("storage"))
            {
                await _logger.Log(LogSeverity.Critical, LogSource.DepressedBot,
                    "The \"storage\" directory didn't exist, so I created it for you.");
                Directory.CreateDirectory("storage");
                return;
            }

            if (Config.Token.IsNullOrEmpty() || Config.Token.EqualsIgnoreCase("token here"))
            {
                await _logger.Log(LogSeverity.Critical, LogSource.DepressedBot,
                    "You can't login to Discord with an invalid token. Fix the token value in the bot's config, then restart.");
                return;
            }

            await Client.LoginAsync(TokenType.Bot, Config.Token.Trim());
            await Client.StartAsync();
            await Client.SetStatusAsync(UserStatus.Online);
            await _handler.InitializeAsync();
            try
            {
                await Task.Delay(-1, Cts.Token);
            }
            catch (TaskCanceledException)
            {
                await ShutdownAsync();
            }
        }

        private async Task ShutdownAsync()
        {
            await Client.SetStatusAsync(UserStatus.Invisible);
            await Client.LogoutAsync();
            await Client.StopAsync();
            Dispose();
            Environment.Exit(0);
        }

        public void Dispose()
        {
            Cts.Dispose();
            ServiceProvider.Dispose();
            Client.Dispose();
        }
    }
}