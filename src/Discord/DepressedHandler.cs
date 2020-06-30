using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DepressedBot.Commands;
using DepressedBot.Data;
using DepressedBot.Data.Objects;
using DepressedBot.Data.Objects.EventArgs;
using Gommon;
using DepressedBot.Services;
using DepressedBot.Extensions;
using Discord;
using Discord.WebSocket;
using Humanizer;
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
        private readonly ReactionService _reaction;
        private readonly ModerationService _moderation;
        private readonly OwoService _owo;
        private readonly CountingService _counting;
        private readonly ConfessionalService _confessional;
        private readonly IServiceProvider _provider;

        public DepressedHandler(DiscordSocketClient client,
            CommandService commandService,
            LoggingService loggingService,
            AutoResponseService autoResponseService,
            DadService dadService,
            ReactionService reactionService,
            ModerationService moderationService,
            OwoService owoService,
            CountingService countingService,
            ConfessionalService confessionalService,
            IServiceProvider serviceProvider)
        {
            _client = client;
            _service = commandService;
            _logger = loggingService;
            _autoResponse = autoResponseService;
            _dad = dadService;
            _reaction = reactionService;
            _moderation = moderationService;
            _owo = owoService;
            _counting = countingService;
            _confessional = confessionalService;
            _provider = serviceProvider;
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
                if (!(s is SocketUserMessage msg)) return;
                if (msg.Author.IsBot) return;
                if (msg.Channel is IDMChannel dm)
                {
                    if (msg.Content.StartsWith($"{Config.CommandPrefix}confess", StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }
                    await dm.SendMessageAsync("I do not support commands via DM.");
                    return;
                }

                var args = new MessageReceivedEventArgs(msg);
                await HandleMessageReceivedAsync(args);
            };
            _client.MessageReceived += async (s) =>
            {
                if (!(s is SocketUserMessage msg)) return;
                if (msg.Author.IsBot) return;
                await _confessional.HandleMessageAsync(new DepressedBotContext(_client, msg, _provider));
            };

        }

        private async Task HandleMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            _ = Executor.ExecuteAsync(async () => await _autoResponse.OnMessageReceivedAsync(args));
            _ = Executor.ExecuteAsync(async () => await _dad.OnMessageReceivedAsync(args));
            _ = Executor.ExecuteAsync(async () => await _reaction.OnMessageReceivedAsync(args));
            _ = Executor.ExecuteAsync(async () => await _moderation.OnMessageReceivedAsync(args));
            _ = Executor.ExecuteAsync(async () => await _owo.OnMessageReceivedAsync(args));
            _ = Executor.ExecuteAsync(async () => await _counting.OnMessageReceivedAsync(args));

            if (CommandUtilities.HasPrefix(args.Message.Content, Config.CommandPrefix, out var cmd))
            {
                var sw = Stopwatch.StartNew();
                var res = await _service.ExecuteAsync(cmd, args.Context, DepressedBot.ServiceProvider);
                sw.Stop();
                if (res is CommandNotFoundResult) return;
                var targetCommand = _service.GetAllCommands()
                                        .FirstOrDefault(x => x.FullAliases.ContainsIgnoreCase(cmd))
                                    ?? _service.GetAllCommands()
                                        .FirstOrDefault(x => x.FullAliases.ContainsIgnoreCase(cmd.Split(' ')[0]));
                await OnCommandAsync(targetCommand, res, args.Context, sw);
            }
        }

        public async Task OnReady(ReadyEventArgs args)
        {
            var guilds = args.Client.Guilds.Count;
            var users = args.Client.Guilds.SelectMany(x => x.Users).DistinctBy(x => x.Id).Count();
            var channels = args.Client.Guilds.SelectMany(x => x.Channels).DistinctBy(x => x.Id).Count();

            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot,
                $"Currently running DepressedBot V{Version.FullVersion}");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, "Use this URL to invite me to your servers:");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, $"{args.Client.GetInviteUrl(true)}");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, $"Logged in as {args.Client.CurrentUser}");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, "Connected to:");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, $"    {"guild".ToQuantity(guilds)}");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, $"    {"user".ToQuantity(users)}");
            await _logger.Log(LogSeverity.Info, LogSource.DepressedBot, $"    {"channel".ToQuantity(channels)}");


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

        public async Task OnCommandAsync(Command c, IResult res, ICommandContext context, Stopwatch sw)
        {
            var ctx = context.Cast<DepressedBotContext>();
            var commandName = ctx.Message.Content.Split(" ")[0];
            var args = ctx.Message.Content.Replace($"{commandName}", "");
            if (string.IsNullOrEmpty(args)) args = "None";
            if (res is FailedResult failedRes)
            {
                await OnCommandFailureAsync(c, failedRes, ctx, args, sw);
                return;
            }

            if (Config.LogAllCommands)
            {
                await _logger.Log(LogSeverity.Info, LogSource.Module,
                    $"|  -Command from user: {ctx.User.Username}#{ctx.User.Discriminator} ({ctx.User.Id})");
                await _logger.Log(LogSeverity.Info, LogSource.Module,
                    $"|     -Command Issued: {c.Name}");
                await _logger.Log(LogSeverity.Info, LogSource.Module,
                    $"|        -Args Passed: {args.Trim()}");
                await _logger.Log(LogSeverity.Info, LogSource.Module,
                    $"|           -In Guild: {ctx.Guild.Name} ({ctx.Guild.Id})");
                await _logger.Log(LogSeverity.Info, LogSource.Module,
                    $"|         -In Channel: #{ctx.Channel.Name} ({ctx.Channel.Id})");
                await _logger.Log(LogSeverity.Info, LogSource.Module,
                    $"|        -Time Issued: {DateTime.Now}");
                await _logger.Log(LogSeverity.Info, LogSource.Module,
                    $"|           -Executed: {res.IsSuccessful} ");
                await _logger.Log(LogSeverity.Info, LogSource.Module,
                    $"|              -After: {sw.Elapsed.Humanize()}");
                await _logger.Log(LogSeverity.Info, LogSource.Module,
                    "-------------------------------------------------");
            }
        }

        private async Task OnCommandFailureAsync(Command c, FailedResult res, DepressedBotContext ctx, string args,
            Stopwatch sw)
        {
            var embed = new EmbedBuilder();
            string reason;
            switch (res)
            {
                case CommandNotFoundResult _:
                    reason = "Unknown command.";
                    break;
                case ExecutionFailedResult efr:
                    reason = $"Execution of this command failed.\nFull error message: {efr.Exception.Message}";
                    await _logger.Log(LogSeverity.Error, LogSource.Module, string.Empty, efr.Exception);
                    break;
                case ChecksFailedResult _:
                    reason = "Insufficient permission.";
                    break;
                case ParameterChecksFailedResult pcfr:
                    reason = $"Checks failed on parameter *{pcfr.Parameter.Name}**.";
                    break;
                case ArgumentParseFailedResult apfr:
                    reason = $"Parsing for arguments failed on argument **{apfr.Parameter?.Name}**.";
                    break;
                case TypeParseFailedResult tpfr:
                    reason =
                        $"Failed to parse type **{tpfr.Parameter.Type.FullName}** from parameter **{tpfr.Parameter.Name}**.";
                    break;
                default:
                    reason = "Unknown error.";
                    break;
            }

            if (reason != "Insufficient permission." && reason != "Unknown command.")
            {
                await embed.AddField("Error in Command:", c.Name)
                    .AddField("Error Reason:", reason)
                    .AddField("Correct Usage", c.SanitizeUsage())
                    .WithAuthor(ctx.User)
                    .WithErrorColor()
                    .SendToAsync(ctx.Channel);

                if (!Config.LogAllCommands) return;

                await _logger.Log(LogSeverity.Error, LogSource.Module,
                    $"|  -Command from user: {ctx.User.Username}#{ctx.User.Discriminator} ({ctx.User.Id})");
                await _logger.Log(LogSeverity.Error, LogSource.Module,
                    $"|     -Command Issued: {c.Name}");
                await _logger.Log(LogSeverity.Error, LogSource.Module,
                    $"|        -Args Passed: {args.Trim()}");
                await _logger.Log(LogSeverity.Error, LogSource.Module,
                    $"|           -In Guild: {ctx.Guild.Name} ({ctx.Guild.Id})");
                await _logger.Log(LogSeverity.Error, LogSource.Module,
                    $"|         -In Channel: #{ctx.Channel.Name} ({ctx.Channel.Id})");
                await _logger.Log(LogSeverity.Error, LogSource.Module,
                    $"|        -Time Issued: {DateTime.Now}");
                await _logger.Log(LogSeverity.Error, LogSource.Module,
                    $"|           -Executed: {res.IsSuccessful} | Reason: {reason}");
                await _logger.Log(LogSeverity.Error, LogSource.Module,
                    $"|              -After: {sw.Elapsed.Humanize()}");
                await _logger.Log(LogSeverity.Error, LogSource.Module,
                    "-------------------------------------------------");
            }
        }
    }
}