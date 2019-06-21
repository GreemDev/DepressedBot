using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using DepressedBot.Services;
using Discord.WebSocket;
using Qmmands;

namespace DepressedBot.Commands.Modules.Utility
{
    public partial class UtilityModule : DepressedBotModule
    {
        public DatabaseService Db { get; set; }

        [Command("OwoScore")]
        [Description("Shows your highest score for the OwO \"feature.\"")]
        [Usage("|prefix|owoscore [user]")]
        public async Task OwoScoreAsync(SocketGuildUser user = null)
        {
            var target = user ?? Context.User;
            Db.GetOwoScore(target, out var score);
            await Context.CreateEmbed($"{target.Mention}'s OwO score is **{score}**!").SendToAsync(Context.Channel);
        }
    }
}
