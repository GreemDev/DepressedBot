using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Humanizer;
using Qmmands;

namespace DepressedBot.Commands.Modules.Utility
{
    public partial class UtilityModule : DepressedBotModule
    {
        [Command("Uptime")]
        [Description("Shows the bot's uptime in a human-friendly fashion.")]
        [Usage("|prefix|uptime")]
        public async Task UptimeAsync()
        {
            await Context
                .CreateEmbed(
                    $"I've been online for **{(DateTime.Now - Process.GetCurrentProcess().StartTime).Humanize(3)}**!")
                .SendToAsync(Context.Channel);
        }
    }
}
