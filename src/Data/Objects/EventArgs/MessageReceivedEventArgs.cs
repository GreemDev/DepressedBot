using System;
using System.Collections.Generic;
using System.Text;
using DepressedBot.Commands;
using Discord;
using Discord.WebSocket;

namespace DepressedBot.Data.Objects.EventArgs
{
    public sealed class MessageReceivedEventArgs : System.EventArgs
    {
        public IUserMessage Message { get; }
        public DepressedBotContext Context { get; }

        public MessageReceivedEventArgs(SocketMessage s)
        {
            Message = s as IUserMessage;
            Context = new DepressedBotContext(Discord.DepressedBot.Client, Message, Discord.DepressedBot.ServiceProvider);
        }
    }
}
