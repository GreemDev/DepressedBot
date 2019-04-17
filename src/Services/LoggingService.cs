using System;
using System.Threading;
using System.Threading.Tasks;
using DepressedBot.Data.Objects;
using DepressedBot.Data.Objects.EventArgs;
using Gommon;
using Discord;
using Console = Colorful.Console;
using Color = System.Drawing.Color;

namespace DepressedBot.Services
{
    [Service("Logging", "The main Service used to handle logging to the bot's console.")]
    public sealed class LoggingService
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        internal async Task Log(LogEventArgs args)
        {
            if (args.LogMessage.Discord.Message.Contains("Preemptive")) return;
            await Log(args.LogMessage.Internal.Severity, args.LogMessage.Internal.Source,
                args.LogMessage.Internal.Message, args.LogMessage.Internal.Exception);
        }

        internal async Task PrintVersion()
        {
            await Log(LogSeverity.Info, LogSource.DepressedBot, $"Currently running DepressedBot V{Version.FullVersion}");
        }

        public async Task Log(LogSeverity s, LogSource src, string message, Exception e = null)
        {
            await _semaphore.WaitAsync();
            DoLog(s, src, message, e);
            _semaphore.Release();
        }

        private void DoLog(LogSeverity s, LogSource src, string message, Exception e = null)
        {
            var (color, value) = VerifySeverity(s);
            Append($"{value} -> ", color);

            (color, value) = VerifySource(src);
            Append($"{value} -> ", color);

            if (!message.IsNullOrWhitespace())
                Append(message, Color.White);

            if (e != null)
                Append($"{e.Message}\n{e.StackTrace}", Color.IndianRed);


            Console.Write(Environment.NewLine);
        }

        private void Append(string m, Color c)
        {
            Console.ForegroundColor = c;
            Console.Write(m);
        }

        private (Color, string) VerifySource(LogSource source)
        {
            switch (source)
            {
                case LogSource.Discord:
                case LogSource.Gateway:
                    return (Color.RoyalBlue, "DSCD");
                case LogSource.DepressedBot:
                    return (Color.Crimson, "CORE");
                case LogSource.Service:
                    return (Color.Gold, "SERV");
                case LogSource.Module:
                    return (Color.LimeGreen, "MDLE");
                case LogSource.Rest:
                    return (Color.Tomato, "REST");
                case LogSource.Unknown:
                    return (Color.Teal, "UNKN");
                default:
                    throw new ArgumentNullException(nameof(source), "source cannot be null.");
            }
        }

        private (Color, string) VerifySeverity(LogSeverity s)
        {
            switch (s)
            {
                case LogSeverity.Critical:
                    return (Color.Maroon, "CRIT");
                case LogSeverity.Error:
                    return (Color.DarkRed, "EROR");
                case LogSeverity.Warning:
                    return (Color.Yellow, "WARN");
                case LogSeverity.Info:
                    return (Color.SpringGreen, "INFO");
                case LogSeverity.Verbose:
                    return (Color.Pink, "VRBS");
                case LogSeverity.Debug:
                    return (Color.SandyBrown, "DEBG");
                default:
                    throw new ArgumentNullException(nameof(s), "s cannot be null.");
            }
        }
    }
}