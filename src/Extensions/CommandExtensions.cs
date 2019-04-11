using DepressedBot.Commands.TypeParsers;
using Discord.WebSocket;
using Qmmands;

namespace DepressedBot.Extensions
{
    public static class CommandExtensions
    {
        internal static void AddTypeParsers(this CommandService service)
        {
            service.AddTypeParser(new UserParser<SocketGuildUser>());
            service.AddTypeParser(new UserParser<SocketUser>());
            service.AddTypeParser(new RoleParser<SocketRole>());
            service.AddTypeParser(new ChannelParser<SocketTextChannel>());
            service.AddTypeParser(new EmoteParser());
            service.AddTypeParser(new BooleanParser(), true);
        }
    }
}
