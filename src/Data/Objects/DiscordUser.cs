using LiteDB;

namespace DepressedBot.Data.Objects
{
    public sealed class DiscordUser
    {
        public ulong Id { get; set; }
        public long OwoScore { get; set; }
    }
}
