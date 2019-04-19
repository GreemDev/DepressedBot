using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Qmmands;

namespace DepressedBot.Commands.Modules.BotOwner
{
    public partial class BotOwnerModule : DepressedBotModule
    {
        [Command("Shutdown")]
        [Description("Forces the bot to shutdown.")]
        [Usage("|prefix|shutdown")]
        [RequireBotOwner]
        public async Task ShutdownAsync()
        {
            await Context.CreateEmbed($"Goodbye! {EmojiService.WAVE}").SendToAsync(Context.Channel);
            Discord.DepressedBot.Cts.Cancel();
        }
    }
}
