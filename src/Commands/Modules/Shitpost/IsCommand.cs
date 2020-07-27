using System;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Discord.WebSocket;
using Qmmands;

namespace DepressedBot.Commands.Modules.Shitpost
{
    public partial class ShitpostModule : DepressedBotModule
    {
        [Command("Is")]
        [Description("Determines if a user has a specific attribute.")]
        [Usage("|prefix|is {@user} {attribute}")]
        public async Task IsAsync(SocketGuildUser user, string attribute)
        {
            var r = new Random();
            var @is = r.Next(0, 2) is 1;
            attribute = attribute.Replace("?", ".");

            var response =
                $"{(@is ? "Yes" : "No")}, {user.Username} is {(@is ? "" : "not ")}{attribute}{(attribute.EndsWith(".") ? "" : ".")}";
            await Context.CreateEmbed(response).SendToAsync(Context.Channel);
        }
    }
}