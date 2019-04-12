using System.Linq;
using DepressedBot.Commands.Attributes;
using DepressedBot.Commands.TypeParsers;
using DepressedBot.Data;
using Discord.WebSocket;
using Qmmands;

namespace DepressedBot.Extensions
{
    public static class CommandExtensions
    {
        public static string SanitizeModuleName(this Module m)
            => m.Name.Replace("Module", string.Empty);

        public static string SanitizeUsage(this Command c)
        {
            var aliases = $"({string.Join("|", c.FullAliases)})";
            var attr = c.Attributes.FirstOrDefault(x => x is UsageAttribute).Cast<UsageAttribute>();
            return (attr?.Usage ?? "No usage provided")
                .Replace(c.Name.ToLower(), (c.FullAliases.Count > 1 ? aliases : c.Name).ToLower())
                .Replace("|prefix|", Config.CommandPrefix);
        }

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
