using System.Threading.Tasks;
using DepressedBot.Data;
using DepressedBot.Data.Objects.EventArgs;
using DepressedBot.Extensions;

namespace DepressedBot.Services
{
    [Service("AutoResponse", "The main Service for handling AutoResponses.")]
    public sealed class AutoResponseService
    {

        public async Task OnMessageReceived(MessageReceivedEventArgs args)
        {
            foreach (var resp in AutoResponses.Responses)
            {
                if (args.Message.Content.EqualsIgnoreCase(resp.Phrase) && resp.Enabled)
                {
                    await args.Context.ReplyAsync(resp.Response);
                }
            }
        }

    }
}
