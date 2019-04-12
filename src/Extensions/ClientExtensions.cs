using System;
using System.Collections.Generic;
using System.Text;
using DepressedBot.Data;
using Discord;
using Discord.WebSocket;

namespace DepressedBot.Extensions
{
    public static class ClientExtensions
    {
        public static string GetInviteUrl(this IDiscordClient client, bool shouldHaveAdmin)
        {
            return shouldHaveAdmin is true
                ? $"https://discordapp.com/oauth2/authorize?client_id={client.CurrentUser.Id}&scope=bot&permissions=8"
                : $"https://discordapp.com/oauth2/authorize?client_id={client.CurrentUser.Id}&scope=bot&permissions=0";
        }

        public static IUser GetOwner(this DiscordSocketClient client) => client.GetUser(Config.Owner);

        public static IGuild GetPrimaryGuild(this DiscordSocketClient client) => client.GetGuild(385902350432206849);
    }
}
