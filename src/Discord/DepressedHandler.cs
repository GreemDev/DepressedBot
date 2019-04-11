using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DepressedBot.Data.Objects;
using DepressedBot.Data.Objects.EventArgs;
using DepressedBot.Extensions;
using DepressedBot.Services;
using Discord;
using Discord.WebSocket;
using Qmmands;

namespace DepressedBot.Discord
{
    internal sealed class DepressedHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly LoggingService _logger;
        private readonly AutoResponseService _autoResponse;

        public DepressedHandler(DiscordSocketClient client,
            CommandService commandService,
            LoggingService loggingService,
            AutoResponseService autoResponseService)
        {
            _client = client;
            _service = commandService;
            _logger = loggingService;
            _autoResponse = autoResponseService;
        }

        public async Task InitializeAsync()
        {
            _service.AddTypeParsers();
            var sw = Stopwatch.StartNew();
            var loaded = _service.AddModules(Assembly.GetEntryAssembly());
            sw.Stop();
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot,
                $"Loaded {loaded.Count} modules and {loaded.Sum(m => m.Commands.Count)} commands loaded in {sw.ElapsedMilliseconds}ms.");
            _client.Log += async (m) => await _logger.Log(new LogEventArgs(m));

            _client.MessageReceived += async (s) =>
            {
                if (!(s is SocketUserMessage msg)) return;
                if (msg.Author.IsBot || msg.Author.IsWebhook) return;
                if (msg.Channel is IDMChannel)
                {
                    await msg.Channel.SendMessageAsync("I do not support commands via DM.");
                    return;
                }

                await HandleMessageReceivedAsync(new MessageReceivedEventArgs(s));
            };

        }

        private async Task HandleMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            await _autoResponse.OnMessageReceived(args);
        }
    }
}
