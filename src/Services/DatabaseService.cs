using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DepressedBot.Data.Objects;
using Discord;
using LiteDB;

namespace DepressedBot.Services
{
    [Service("Database", "The main Service for DepressedBot's database.")]
    public sealed class DatabaseService
    {
        public static readonly LiteDatabase Database = new LiteDatabase("storage/DepressedBot.db");

        public void GetOwoScore(IUser user, out long score)
            => score = GetUser(user.Id).OwoScore;

        public DiscordUser GetUser(ulong user)
        {
            var coll = Database.GetCollection<DiscordUser>("users");
            var record = coll.FindOne(x => x.Id == user);
            if (!(record is null)) return record;

            var newRecord = Create(user);
            coll.Insert(newRecord);
            return newRecord;

        }

        public void UpdateUser(DiscordUser newRecord)
        {
            var coll = Database.GetCollection<DiscordUser>("users");
            coll.EnsureIndex(s => s.Id, true);
            coll.Update(newRecord);
        } 

        private DiscordUser Create(ulong user)
            => new DiscordUser
            {
                Id = user,
                OwoScore = 0
            };
    }
}
