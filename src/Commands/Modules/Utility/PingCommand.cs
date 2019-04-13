using System.Diagnostics;
using DepressedBot.Extensions;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using Qmmands;

namespace DepressedBot.Commands.Modules.Utility
{
    public partial class UtilityModule : DepressedBotModule
    {
        [Command("Ping")]
        [Description("Show the Gateway latency to Discord.")]
        [Usage("|prefix|ping")]
        public async Task PingAsync()
        {
            var e = Context.CreateEmbedBuilder("Pinging...");
            var sw = new Stopwatch();
            sw.Start();
            var msg = await e.SendToAsync(Context.Channel);
            sw.Stop();
            await msg.ModifyAsync(x =>
            {
                e.WithDescription(
                    $"{EmojiService.CLAP} **Ping**: {sw.ElapsedMilliseconds}ms \n" +
                    $"{EmojiService.OK_HAND} **API**: {Context.Client.Latency}ms");
                x.Embed = e.Build();
            });
        }
    }
}
