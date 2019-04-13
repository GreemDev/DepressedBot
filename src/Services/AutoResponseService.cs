using System.Linq;
using System.Threading.Tasks;
using DepressedBot.Data;
using DepressedBot.Data.Objects.EventArgs;
using Gommon;

namespace DepressedBot.Services
{
    [Service("AutoResponse", "The main Service for handling AutoResponses.")]
    public sealed class AutoResponseService
    {
        public async Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            if (Config.IgnoredCategoryIds.Contains(args.Context.Channel.CategoryId ?? 0)) return;
            if (AutoResponses.Responses.Any(x => args.Message.Content.ContainsIgnoreCase(x.Phrase)))
            {
                var m = await args.Context.ReplyAsync(AutoResponses.Responses
                    .FirstOrDefault(x => args.Message.Content.ContainsIgnoreCase(x.Phrase)).Response);
                await ExecutorUtil.ExecuteAfterDelayAsync(4000, async () => await m.DeleteAsync());
            }
        }
    }
}