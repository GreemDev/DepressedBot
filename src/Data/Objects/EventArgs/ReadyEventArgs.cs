
using Discord.WebSocket;

namespace DepressedBot.Data.Objects.EventArgs
{
    public sealed class ReadyEventArgs : System.EventArgs
    {
        public DiscordSocketClient Client { get; }

        public ReadyEventArgs(DiscordSocketClient client)
        {
            Client = client;
        }
    }
}
