using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DepressedBot.Data;
using DepressedBot.Data.Objects.EventArgs;
using DepressedBot.Extensions;

namespace DepressedBot.Services
{
    [Service("AutoResponse", "The main Service for handling AutoResponses.")]
    public sealed class AutoResponseService
    {
        public async Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            foreach (var resp in AutoResponses.Responses)
            {
                if (args.Message.Content.EqualsIgnoreCase(resp.Phrase) && resp.Enabled)
                {
                    var m = await args.Context.ReplyAsync(resp.Response);
                    await Task.Delay(4000)
                        .ContinueWith(async _ => await m.DeleteAsync());
                }
            }
        }
    }
}