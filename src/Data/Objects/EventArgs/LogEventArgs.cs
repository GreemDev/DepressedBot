﻿using Discord;

namespace DepressedBot.Data.Objects.EventArgs
{
    public sealed class LogEventArgs : System.EventArgs
    {
        public string Message { get; }
        public string Source { get; }
        public LogSeverity Severity { get; }
        public (LogMessage Internal, global::Discord.LogMessage Discord) LogMessage { get; }

        public LogEventArgs(global::Discord.LogMessage message)
        {
            Message = message.Message;
            Source = message.Source;
            Severity = message.Severity;
            LogMessage = (Objects.LogMessage.FromDiscordLogMessage(message), message);
        }
    }
}
