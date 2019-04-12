using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DepressedBot.Data;
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
        private readonly bool _shouldStream =
            Config.Streamer.EqualsIgnoreCase("streamer here") || Config.Streamer.IsNullOrWhitespace();

        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly LoggingService _logger;
        private readonly AutoResponseService _autoResponse;
        private readonly DadService _dad;

        public DepressedHandler(DiscordSocketClient client,
            CommandService commandService,
            LoggingService loggingService,
            AutoResponseService autoResponseService,
            DadService dadService)
        {
            _client = client;
            _service = commandService;
            _logger = loggingService;
            _autoResponse = autoResponseService;
            _dad = dadService;
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
            _client.Ready += async () => await OnReady(new ReadyEventArgs(_client));

            _client.MessageReceived += async (s) =>
            {
                if (!(s is IUserMessage msg)) return;
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
            await _autoResponse.OnMessageReceivedAsync(args);
            await _dad.OnMessageReceivedAsync(args);
        }

        public async Task OnReady(ReadyEventArgs args)
        {
            var guilds = args.Client.Guilds.Count;
            var users = args.Client.Guilds.SelectMany(x => x.Users).DistinctBy(x => x.Id).Count();
            var channels = args.Client.Guilds.SelectMany(x => x.Channels).DistinctBy(x => x.Id).Count();

            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, $"Currently running DepressedBot V{Version.FullVersion}");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, "Use this URL to invite me to your servers:");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, $"{args.Client.GetInviteUrl(true)}");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, $"Logged in as {args.Client.CurrentUser}");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, "Connected to:");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot,
                $"    {guilds} server{(guilds.ShouldBePlural() ? "s" : "")}");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot,
                $"    {users} user{(users.ShouldBePlural() ? "s" : "")}");

            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot,
                $"    {channels} channel{(channels.ShouldBePlural() ? "s" : "")}");


            if (_shouldStream)
            {
                await args.Client.SetGameAsync(Config.Game);
                await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, $"Set the bot's game to {Config.Game}.");
            }
            else
            {
                var twitchUrl = $"https://twitch.tv/{Config.Streamer}";
                await args.Client.SetGameAsync(Config.Game, twitchUrl, ActivityType.Streaming);
                await _logger.Log(LogSeverity.Info, LogSource.DepressedBot,
                    $"Set the bot's game to \"{ActivityType.Streaming} {Config.Game}, at {twitchUrl}\".");
            }
        }
    }
}