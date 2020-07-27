using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using Discord;
using Discord.WebSocket;
using Qmmands;

namespace DepressedBot.Commands.Modules.Utility
{
    public partial class UtilityModule : DepressedBotModule
    {
        [Command("NewMusicChannel", "Nmc")]
        [Description("Creates a new music channel with the specified owner and name.")]
        [Usage("|prefix|newmusicchannel {user} {name}")]
        [RequireAdmin]
        public async Task NewMusicChannelAsync(SocketGuildUser user, [Remainder]string name)
        {
            var channel = await Context.Guild.CreateTextChannelAsync(name.Replace(" ", "-").ToLower(), 
                x => x.CategoryId = 542855804727066636);
            await channel.AddPermissionOverwriteAsync(user,
                new OverwritePermissions(manageChannel: PermValue.Allow, manageMessages: PermValue.Allow,
                    viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));
            await channel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole,
                new OverwritePermissions(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));

            await channel.SendMessageAsync($"Welcome to your personal music channel, {user.Mention}! " +
                                           "Feel free to post whatever music you like here. " +
                                           "Consider modifying this channel's topic to whitelist users messaging here, or simply put `whitelist-off` to allow anyone to message here. " +
                                           "Have fun!");
        }
    }
}
