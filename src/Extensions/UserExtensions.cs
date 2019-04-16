using System.Linq;
using DepressedBot.Data;
using Discord;

namespace DepressedBot.Extensions
{
    public static class UserExtensions
    {

        public static bool IsAdmin(this IGuildUser user)
            => user.RoleIds.Any(x => x == 385903172176183298);

        public static bool IsBotOwner(this IUser user)
            => user.Id == Config.Owner;

    }
}
