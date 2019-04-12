using DepressedBot.Commands;
using Discord;
using Discord.WebSocket;

namespace DepressedBot.Data.Objects.EventArgs
{
    public sealed class MessageReceivedEventArgs : System.EventArgs
    {
        public IUserMessage Message { get; }
        public DepressedBotContext Context { get; }

        public MessageReceivedEventArgs(SocketUserMessage s)
        {
            Message = s;
            Context = new DepressedBotContext(Discord.DepressedBot.Client, Message, Discord.DepressedBot.ServiceProvider);
        }
    }
}
