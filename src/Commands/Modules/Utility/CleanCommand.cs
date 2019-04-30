using System.Linq;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Discord;
using Gommon;
using Humanizer;
using Qmmands;

namespace DepressedBot.Commands.Modules.Utility
{
    public partial class UtilityModule : DepressedBotModule
    {
        [Command("Clean")]
        [Description(
            "Cleans the bot's messages in the current channel. Checks the last 100 messages to limit REST requests to Discord.")]
        [Usage("|prefix|clean")]
        public async Task CleanAsync()
        {
            var messages =
                (await Context.Channel.GetMessagesAsync().FlattenAsync()).Where(x =>
                    x.Author.Id == Context.Client.CurrentUser.Id).ToList();
            await Context.Channel.DeleteMessagesAsync(messages);
            var m = await Context.CreateEmbed($"Deleted {"message".ToQuantity(messages.Count)}.")
                .SendToAsync(Context.Channel);
            await ExecutorUtil.ExecuteAfterDelayAsync(4000, async () =>
            {
                await Context.Message.DeleteAsync();
                await m.DeleteAsync();
            });
        }
    }
}