using LiteDB;

namespace DepressedBot.Data.Objects
{
    public sealed class DiscordUser
    {
        public ObjectId ObjId { get; set; }
        public ulong Id { get; set; }
        public long OwoScore { get; set; }
    }
}
