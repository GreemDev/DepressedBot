using System.Threading.Tasks;
using DepressedBot.Data.Objects.EventArgs;
using DepressedBot.Extensions;
using Discord;

namespace DepressedBot.Services
{
    [Service("Reaction", "The main Service for handling certain inputs to have reactions added to them.")]
    public sealed class ReactionService
    {
        private readonly EmojiService _emoji;
        private readonly LoggingService _logger;

        public ReactionService(EmojiService emojiService, LoggingService loggingService)
        {
            _emoji = emojiService;
            _logger = loggingService;
        }

        public async Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            if (args.Message.Author.IsBot) return;
            var ctx = args.Context;
            if (ctx.Message.Content.ContainsIgnoreCase("lmao"))
            {
                await ctx.ReactAsync(_emoji.REGIONAL_INDICATOR_L);
                await ctx.ReactAsync(_emoji.REGIONAL_INDICATOR_M);
                await ctx.ReactAsync(_emoji.REGIONAL_INDICATOR_A);
                await ctx.ReactAsync(_emoji.REGIONAL_INDICATOR_O);

            }
            else if (ctx.Message.Content.ContainsIgnoreCase("lmfao"))
            {
                await ctx.ReactAsync(_emoji.REGIONAL_INDICATOR_L);
                await ctx.ReactAsync(_emoji.REGIONAL_INDICATOR_M);
                await ctx.ReactAsync(_emoji.REGIONAL_INDICATOR_F);
                await ctx.ReactAsync(_emoji.REGIONAL_INDICATOR_A);
                await ctx.ReactAsync(_emoji.REGIONAL_INDICATOR_O);
            }
        }
    }
}
