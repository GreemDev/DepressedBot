using System;
using System.Collections.Generic;
using System.Text;
using DepressedBot.Commands;
using DepressedBot.Services;
using Discord.WebSocket;
using Qmmands;

namespace DepressedBot.Data.Objects
{
    public sealed class EvalObjects
    {
        public DepressedBotContext Context { get; set; }
        public DiscordSocketClient Client { get; set; }
        public LoggingService Logger { get; set; }
        public CommandService CommandService { get; set; }
        public EmojiService EmojiService { get; set; }
    }
}
