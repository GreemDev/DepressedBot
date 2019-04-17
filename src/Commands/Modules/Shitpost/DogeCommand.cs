using System;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Qmmands;

namespace DepressedBot.Commands.Modules.Shitpost
{
    public partial class ShitpostModule : DepressedBotModule
    {
        [Command("Doge")]
        [Description("Gets a doge meme from i.kuro.mu/doge.")]
        [Usage("|prefix|doge")]
        public async Task DogeAsync()
        {
            await Context.CreateEmbedBuilder()
                .WithImageUrl($"https://i.kuro.mu/doge/{new Random().Next(1, 203)}.png")
                .SendToAsync(Context.Channel);
        }
    }
}
